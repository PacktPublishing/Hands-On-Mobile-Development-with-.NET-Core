using System;
using System.ComponentModel;

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
#if __IOS__
                return "iOS";
#elif __ANDROID__
                return "Android";
#endif
            }
        }
    }
}