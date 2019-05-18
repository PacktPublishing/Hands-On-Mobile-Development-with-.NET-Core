using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FirstXamarinFormsApplication.Client.Diagnostics
{
    public abstract class TelemetryEvent
    {
        public TelemetryEvent()
        {
            Properties = new Dictionary<string, string>();
        }

        public TelemetryEvent(string eventName) : this()
        {
            Name = eventName;
        }

        public string Name { get; private set; }

        public Exception Exception { get; set; }

        public virtual Dictionary<string, string> Properties { get; set; }
    }

    public class ProductsRequestEvent : ChronoTelemetryEvent
    {
        public ProductsRequestEvent() : base("ProductsServiceRequest")
        {
        }
    }

    public abstract class ChronoTelemetryEvent : TelemetryEvent
{
    public ChronoTelemetryEvent(string eventName): base(eventName)
    {
        Properties.Add(nameof(Elapsed), 0.ToString());
    }

    public double Elapsed
    {
        get
        {
            return double.Parse(Properties[nameof(Elapsed)]);
        }

        set
        {
            Properties[nameof(Elapsed)] = value.ToString();
        }
    }
}

public class TelemetryTracker<TEvent> : IDisposable where TEvent : ChronoTelemetryEvent
{
    private readonly DateTime _executionStart = DateTime.Now;

    public TelemetryTracker(TEvent @event)
    {
            Event = @event;
    }

        public TEvent Event { get; }

        public void Dispose()
    {
        var executionTime = DateTime.Now - _executionStart;
            Event.Elapsed = executionTime.TotalMilliseconds;

        // The submission of the event can as well be moved out of the tracker
        AppTelemetryRouter.Instance?.TrackEvent(Event);
    }
}

    public class LoginEvent: TelemetryEvent
    {
        public LoginEvent() : base("Login")
        {
            Properties.Add(nameof(Result), string.Empty);
        }

        public string Result 
        { 
            get 
            {
                return Properties[nameof(Result)];
            } 

            set 
            {
                Properties[nameof(Result)] = value;
            } 
        }
    }

public class HomePageEvent : TelemetryEvent
{
    public HomePageEvent() : base("HomeView")
    {
        Properties.Add(nameof(LoadedItems), string.Empty);
    }

    public string LoadedItems
    {
        get
        {
            return Properties[nameof(LoadedItems)];
        }

        set
        {
            Properties[nameof(LoadedItems)] = value;
        }
    }
}

    public class DetailsPageEvent : TelemetryEvent
    {
        public DetailsPageEvent() : base("DetailsView")
        {
            Properties.Add(nameof(SelectedItem), string.Empty);
        }

        public string SelectedItem
        {
            get
            {
                return Properties[nameof(SelectedItem)];
            }

            set
            {
                Properties[nameof(SelectedItem)] = value;
            }
        }
    }

    public class AppCenterTelemetryWriter : ITelemetryWriter
    {
        public string Name => "Telemetry Container";

        public void Initialize()
        {
            AppCenter.Start("5faaabe4-8738-44b9-8b4c-e319295e51cd", typeof(Analytics), typeof(Crashes));
        }

        public void TrackEvent(TelemetryEvent @event)
        {
            if (@event.Exception != null)
            {
                TrackError(@event);
                return;
            }

            Analytics.TrackEvent(@event.Name, @event.Properties);
        }

        public void TrackError(TelemetryEvent @event)
        {
            Crashes.TrackError(@event.Exception, @event.Properties);
        }
    }

public interface ITelemetryWriter
{
    string Name { get; }

    void TrackEvent(TelemetryEvent @event);

    void TrackError(TelemetryEvent @event);
}

public class AppTelemetryRouter : ITelemetryWriter
{
    private static AppTelemetryRouter _instance;

    private List<ITelemetryWriter> _telemetryWriters = new List<ITelemetryWriter>();

    private AppTelemetryRouter()
    { 
    }

    public static AppTelemetryRouter Instance 
    { 
        get 
        {
            if(_instance == null)
            {
                _instance = new AppTelemetryRouter(); 
            }

            return _instance;
        } 
    }

    public string Name => "Telemetry Container";

    public void RegisterWriter(ITelemetryWriter telemetryWriter)
    {
        if(_telemetryWriters.Any(tw=>tw.Name == telemetryWriter.Name))
        {
            throw new InvalidOperationException($"Already registered Telemetry Writer for {telemetryWriter.Name}"); 
        }

        _telemetryWriters.Add(telemetryWriter);
    }

    public void RemoveWriter(string name)
    {
        if(_telemetryWriters.Any(tw => tw.Name == name))
        {
            var removalItems = _telemetryWriters.First(tw => tw.Name == name);

            _telemetryWriters.Remove(removalItems);
        }
    }

    public void TrackEvent(TelemetryEvent @event)
    {
        _telemetryWriters.ForEach(tw => tw.TrackEvent(@event));
    }

    public void TrackError(TelemetryEvent @event)
    {
        _telemetryWriters.ForEach(tw => tw.TrackError(@event));
    }
}
}
