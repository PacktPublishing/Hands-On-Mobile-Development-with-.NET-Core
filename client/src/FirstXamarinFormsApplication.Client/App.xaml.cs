using System;
using System.Runtime.CompilerServices;

using FirstXamarinFormsApplication.Client;
using FirstXamarinFormsApplication.Client.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: InternalsVisibleTo("NetCore.Mobile.Tests")]

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace FirstXamarinFormsApplication
{
	public partial class App : Application
	{
        public App ()
        {
        	InitializeComponent();

                    var navigationPage = new MasterDetail();

                    //MainPage = navigationPage;

            var appCenterTelemetry = new AppCenterTelemetryWriter();
            appCenterTelemetry.Initialize();
            AppTelemetryRouter.Instance.RegisterWriter(appCenterTelemetry);

                    MainPage = new LoginView();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public static AppCenterTelemetryWriter Telemetry { get; set; }
    }
}
