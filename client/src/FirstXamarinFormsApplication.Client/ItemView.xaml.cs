using System;
using System.Collections.Generic;
using FirstXamarinFormsApplication.Client.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FirstXamarinFormsApplication.Client
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class ItemView : ContentPage
    {
        public ItemView()
        {
            InitializeComponent();

            if(DesignMode.IsDesignModeEnabled)
            {
                BindingContext = new ItemViewModel() { ReleaseDate = null, HasFeature1=true, IsReleased=true, Title = "Item 1", IsHtml=true, Description = "<b>First</b> Item short description", Image = "https://picsum.photos/800?image=0" };
            }
        }

protected override void OnAppearing()
{
    base.OnAppearing();

    var viewModel = (ItemViewModel)BindingContext;

            AppTelemetryRouter.Instance.TrackEvent(new DetailsPageEvent() { SelectedItem = viewModel.Title });
}
    }
}
