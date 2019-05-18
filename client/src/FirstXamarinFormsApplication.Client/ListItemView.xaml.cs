using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using FirstXamarinFormsApplication.Client.Diagnostics;

using Newtonsoft.Json;

using Xamarin.Forms;

using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace FirstXamarinFormsApplication.Client
{
    public partial class ListItemView : ContentPage
    {
        ListItemViewModel _viewModel;

        public ListItemView()
        {
            InitializeComponent();

            _viewModel = new ListItemViewModel(new ApiClient(), null);

            BindingContext = _viewModel;

            var picker = new Xamarin.Forms.Picker();
            picker.On<iOS>().SetUpdateMode(UpdateMode.WhenFinished);
        }

protected override void OnAppearing()
{
    base.OnAppearing();

            //AppTelemetryRouter.Instance.TrackEvent(new HomePageEvent() { LoadedItems = _viewModel.Items.Count.ToString() });

            //_viewModel.LoadProducts();
        }

        private void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var itemView = new ItemView();
            itemView.BindingContext = e.Item;
            Navigation.PushAsync(itemView);
        }
    }

    public class BaseBindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SendPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ListItemViewModel : BaseBindableObject //, IPageLifecycleHandler
    {
        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();

        private readonly IApiClient _serviceClient;

        private readonly INavigationService _navigationService;

        public ListItemViewModel(IApiClient apiClient, INavigationService navigationService)
        {
            _serviceClient = apiClient;

            _navigationService = navigationService;

            ItemTapped = new Command<ItemViewModel>(_ => NavigateToItem(_));

            if (_serviceClient != null)
            {
                (Initialized = LoadProducts()).ConfigureAwait(false);
            }
        }

        internal Task Initialized { get; set; }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                SendPropertyChanged();
            }
        }

        public ICommand ItemTapped { get; }

        //public Task OnAppearing()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task OnDisappearing()
        //{
        //    throw new NotImplementedException();
        //}

        internal async Task LoadProducts()
        {
            using (var telemetry = new TelemetryTracker<ProductsRequestEvent>(new ProductsRequestEvent()))
            {
                try
                {
                    var result = await _serviceClient.RetrieveProductsAsync();

                    Items = new ObservableCollection<ItemViewModel>(result.Select(item => ItemViewModel.FromDto(item)));
                }
                catch (Exception ex)
                {
                    telemetry.Event.Exception = ex;
                }
            }
        }

        internal async Task NavigateToItem(ItemViewModel viewModel)
        {
            if (viewModel != null && _navigationService != null)
            {
                if (viewModel.IsReleased)
                {
                    if (await _navigationService.NavigateToViewModel(viewModel))
                    {
                        return;
                    }
                }
                else
                {
                    await _navigationService.ShowMessage("The product has not been released yet");
                    return;
                }
            }

            throw new InvalidOperationException("Target view model or navigation service is null");
        }
    }

    public class ApiClient : IApiClient
    {
        public async Task<IEnumerable<Product>> RetrieveProductsAsync()
        {
            await Task.Delay(new Random(200).Next(3000));

            var results = new List<Product>();

            results.Add(new Product { Title = "First Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "First Item short description", ImageUrl = "https://picsum.photos/800?image=0", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Second Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "<b>Here</b> is some <u>HTML</u>", ImageUrl = "https://picsum.photos/800?image=1", HasFeature1 = false, HasFeature2 = true });
            results.Add(new Product { Title = "Third Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Third Item short description.", ImageUrl = "https://picsum.photos/800?image=2", HasFeature1 = false, HasFeature2 = false });
            results.Add(new Product { Title = "Fourth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Fourth Item short description.", ImageUrl = "https://picsum.photos/800?image=3", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Fifth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Fifth Item short description.", ImageUrl = "https://picsum.photos/800?image=4", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Sixth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Sixth Item short description.", ImageUrl = "https://picsum.photos/800?image=5", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Seventh Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Seventh Item short description.", ImageUrl = "https://picsum.photos/800?image=6", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Eigth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Eigth Item short description.", ImageUrl = "https://picsum.photos/800?image=7", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Nineth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Nineth Item short description.", ImageUrl = "https://picsum.photos/800?image=8", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Tenth Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Tenth Item short description.", ImageUrl = "https://picsum.photos/800?image=9", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Eleventh Item", ReleaseDate = new DateTime(2018, 10, 10), Description = "Eleventh Item short description.", ImageUrl = "https://picsum.photos/800?image=10", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Twelveth Item", Description = "Twelveth Item short description.", ImageUrl = "https://picsum.photos/800?image=11", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Thirteenth Item", Description = "Thirteenth Item short description.", ImageUrl = "https://picsum.photos/800?image=12", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Fourteenth Item", Description = "Fourteenth Item short description.", ImageUrl = "https://picsum.photos/800?image=13", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Fifteenth Item", Description = "Fifteenth Item short description.", ImageUrl = "https://picsum.photos/800?image=14", HasFeature1 = true, HasFeature2 = true });
            results.Add(new Product { Title = "Last Item", Description = "Last Item short description.", ImageUrl = "https://picsum.photos/800?image=15", HasFeature1 = true, HasFeature2 = true });

            return results;
        }
    }

    public interface IApiClient
    {
        Task<IEnumerable<Product>> RetrieveProductsAsync();
    }

    public interface INavigationService
    {
        Task<bool> NavigateToViewModel<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseBindableObject;

        Task ShowMessage(string message);
    }

    public interface IPageLifecycleHandler
    {
        Task OnAppearing();

        Task OnDisappearing(); 
    }

    public class ItemViewModel : BaseBindableObject
    {
        private Command _changeTitleCommand;

        private string _title;

        public string Title 
        { 
            get 
            {
                return _title; 
            } 
            set
            { 
                if (_title != value) 
                { 
                    _title = value; 
                    SendPropertyChanged(); 
                }
            } 
        }

        public bool IsHtml { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public ICommand ChangeTitleCommand
        {
            get
            {
                if(_changeTitleCommand == null)
                {
                    _changeTitleCommand = new Command(() => { Title = $"Changed {Title}"; });
                }

                return _changeTitleCommand;
            }
        }

        public bool HasFeature1 { get; set; }

        public bool HasFeature2 { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public bool IsReleased { get; set; }

        public static ItemViewModel FromDto(Product product)
        {
            if (product == null)
            {
                return null;
            }

            var target = new ItemViewModel();
            target.Title = product.Title;
            target.Description = product.Description;
            target.ReleaseDate = product.ReleaseDate;

            if (DateTime.Now >= target?.ReleaseDate)
            {
                target.IsReleased = true;
            }

            // TODO: Use Regular Expression to check for HTML
            if (target.Description.Contains("<b>"))
            {
                target.IsHtml = true;
            }

            target.Image = product.ImageUrl;
            target.HasFeature1 = product.HasFeature1;
            target.HasFeature2 = product.HasFeature2;

            return target;
        }
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("hasFeature1")]
        public bool HasFeature1 { get; set; }

        [JsonProperty("hasFeature2")]
        public bool HasFeature2 { get; set; }
    }
}
