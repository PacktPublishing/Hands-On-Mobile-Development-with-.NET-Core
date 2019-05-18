using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication
{
    public class MainPageViewModel
    {
        public MainPageViewModel()
        {
        }

public string Platform
{
    get

    {
        if (Device.RuntimePlatform.Equals(Device.Android))
        {
            return "Android";
        }
        else if (Device.RuntimePlatform.Equals(Device.iOS))
        {
            return "iOS";
        }
        else if (Device.RuntimePlatform.Equals(Device.UWP))
        {
            return "Universal Windows Platform";
        }
        else
        {
            return "Unknown";
        }
    }
}
    }
}