using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.NotificationHubs;

namespace NetCore.Web.UsersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private NotificationHubClient _hub;

        public NotificationController()
        {
            // Initialize the Notification Hub
            NotificationHubClient _hub = NotificationHubClient.CreateClientFromConnectionString("", "");
        }

        public async Task<IActionResult> Send(NotificationRequest request)
        {
            if (request.Message is SimpleNotificationMessage simpleMessage)
            {
                foreach (var message in simpleMessage.GetPlatformMessages())
                {
                    switch (message.Item1)
                    {
                        case "wns":
                            await _hub.SendWindowsNativeNotificationAsync(message.Item2, 
                                $"username:{request.Destination}");
                            break;
                        case "aps":
                            await _hub.SendAppleNativeNotificationAsync(message.Item2,
                                $"username:{request.Destination}");
                            break;
                        case "fcm":
                            await _hub.SendFcmNativeNotificationAsync(message.Item2, 
                                $"username:{request.Destination}");
                            break;;
                    }
                }

            }
            else if (request.Message is TemplateNotificationMessage templateMessage)
            {
                await _hub.SendTemplateNotificationAsync(templateMessage.Parameters, $"username:{request.Destination}");
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(DeviceRegistration device)
        {
            // New registration, execute cleanup
            if (device.RegistrationId == null && device.Handle != null)
            {
                var registrations = await _hub.GetRegistrationsByChannelAsync(device.Handle, 100);

                foreach (var registration in registrations)
                {
                    await _hub.DeleteRegistrationAsync(registration);
                }

                device.RegistrationId = await _hub.CreateRegistrationIdAsync();
            }

            // ready to registration
            // ...

            RegistrationDescription deviceRegistration = null;

            switch (device.Platform)
            {
                // ...
                case "apns":
                    deviceRegistration = new AppleRegistrationDescription(device.Handle);
                    break;
                //...
            }

            deviceRegistration.RegistrationId = device.RegistrationId;

            deviceRegistration.Tags = new HashSet<string>(device.Tags);

            // Get the user email depending on the current identity provider
            deviceRegistration.Tags.Add($"username:{GetCurrentUser()}");

            await _hub.CreateOrUpdateRegistrationAsync(deviceRegistration);

            var deviceInstallation = new Installation();
            // ... populate fields
            deviceInstallation.Templates = new Dictionary<string, InstallationTemplate>();
            deviceInstallation.Templates.Add("type:Welcome", new InstallationTemplate
            {
                Body = "{\"aps\": {\"alert\" : \"Hi ${FullName} welcome to Auctions!\" }}"
            });

            return Ok();
        }

        public string GetCurrentUser()
        {
            // TODO:
            return string.Empty;
        }
    }

    public class NotificationRequest
    {
        public BaseNotificationMessage Message { get; set; }

        public string Destination { get; set; }
    }

    public abstract class BaseNotificationMessage
    {

    }

    public class TemplateNotificationMessage : BaseNotificationMessage
    {
        public string TemplateTag { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }

    public class SimpleNotificationMessage : BaseNotificationMessage
    {
        public string Message { get; set; }

        public IEnumerable<(string, string)> GetPlatformMessages()
        {
            yield return ("wns",
                             @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + Message +
                             "</text></binding></visual></toast>");

            yield return ("apns", "{\"aps\":{\"alert\":\"" + Message + "\"}}");

            yield return ("fcm", "{\"data\":{\"message\":\"" + Message + "\"}}");
        }
    }

    public class DeviceRegistration
    {
        public string RegistrationId { get; set; } // Registration Id
        public string Platform { get; set; } // wns, apns, fcm
        public string Handle { get; set; } // token or uri
        public string[] Tags { get; set; } // any additional tags
    }

    public class DeviceInstallation
    {
        public string InstallationId { get; set; }
        public string Platform { get; set; }
        public string PushChannel { get; set; }
        public string[] Tags { get; set; }
    }
}