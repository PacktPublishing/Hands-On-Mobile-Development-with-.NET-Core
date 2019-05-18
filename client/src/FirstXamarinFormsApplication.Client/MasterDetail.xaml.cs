using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client
{
    public partial class MasterDetail : MasterDetailPage
    {
        private ListItemViewModel _dataSource = null;

        public MasterDetail()
        {
            InitializeComponent();

            var list = new List<NavigationItem>();
            list.Add(new NavigationItem { Id = -1, Title = "List", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 0, Title = "First", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 1, Title = "Second", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 2, Title = "Third", Icon = "Xamarin.png" });
            list.Add(new NavigationItem { Id = 3, Title = "Fourth", Icon = "Xamarin.png" });

            BindingContext = list;

            _dataSource = new ListItemViewModel(null, null);

            _dataSource.Items.Add(new ItemViewModel { Title = "First Item", Description = "First Item short description", Image = "https://picsum.photos/800?image=0" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Second Item", Description = "Second Item short description.", Image = "https://picsum.photos/800?image=1" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Third Item", Description = "Third Item short description.", Image = "https://picsum.photos/800?image=2" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Fourth Item", Description = "Fourth Item short description.", Image = "https://picsum.photos/800?image=3" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Fifth Item", Description = "Fifth Item short description.", Image = "https://picsum.photos/800?image=4" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Sixth Item", Description = "Sixth Item short description.", Image = "https://picsum.photos/800?image=5" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Seventh Item", Description = "Seventh Item short description.", Image = "https://picsum.photos/800?image=6" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Eigth Item", Description = "Eigth Item short description.", Image = "https://picsum.photos/800?image=7" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Nineth Item", Description = "Nineth Item short description.", Image = "https://picsum.photos/800?image=8" });
            _dataSource.Items.Add(new ItemViewModel { Title = "Tenth Item", Description = "Tenth Item short description.", Image = "https://picsum.photos/800?image=9" });

        }

private void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
{
    if(e.Item is NavigationItem item)
    {
        Page detailPage = null;

        if(item.Id >= 0)
        {
            detailPage = new ItemView();
            detailPage.BindingContext = _dataSource.Items[item.Id];
        }
        else
        {
            detailPage = new ListItemView();
            detailPage.BindingContext = _dataSource;
        }

        this.Detail = new NavigationPage(detailPage);

        this.IsPresented = false;
    }
}
    }

    public class NavigationItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }
    }
}
