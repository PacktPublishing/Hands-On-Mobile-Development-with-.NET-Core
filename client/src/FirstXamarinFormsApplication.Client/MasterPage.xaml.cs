using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client
{
    public partial class MasterPage1 : ContentPage
    {
        public MasterPage1()
        {
            InitializeComponent();

            var list = new List<NavigationItem>();
            list.Add(new NavigationItem { Id = -1, Title = "List", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 0, Title = "First", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 1, Title = "Second", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 2, Title = "Third", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 3, Title = "Fourth", Icon = "Xamarin.png" });

            BindingContext = list;
        }
    }
}
