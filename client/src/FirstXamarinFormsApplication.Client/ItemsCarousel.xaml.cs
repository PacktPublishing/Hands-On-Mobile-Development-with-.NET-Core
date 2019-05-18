using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client
{
    public partial class ItemsCarousel : CarouselPage
    {
        public ItemsCarousel()
        {
            InitializeComponent();

            var viewModel = new ListItemViewModel(null, null);

            viewModel.Items.Add(new ItemViewModel { Title = "First Item", Description = "First Item short description", Image = "https://picsum.photos/800?image=0" });
            viewModel.Items.Add(new ItemViewModel { Title = "Second Item", Description = "Second Item short description.", Image = "https://picsum.photos/800?image=1" });
            viewModel.Items.Add(new ItemViewModel { Title = "Third Item", Description = "Third Item short description.", Image = "https://picsum.photos/800?image=2" });
            viewModel.Items.Add(new ItemViewModel { Title = "Fourth Item", Description = "Fourth Item short description.", Image = "https://picsum.photos/800?image=3" });
            viewModel.Items.Add(new ItemViewModel { Title = "Fifth Item", Description = "Fifth Item short description.", Image = "https://picsum.photos/800?image=4" });
            viewModel.Items.Add(new ItemViewModel { Title = "Sixth Item", Description = "Sixth Item short description.", Image = "https://picsum.photos/800?image=5" });
            viewModel.Items.Add(new ItemViewModel { Title = "Seventh Item", Description = "Seventh Item short description.", Image = "https://picsum.photos/800?image=6" });
            viewModel.Items.Add(new ItemViewModel { Title = "Eigth Item", Description = "Eigth Item short description.", Image = "https://picsum.photos/800?image=7" });
            viewModel.Items.Add(new ItemViewModel { Title = "Nineth Item", Description = "Nineth Item short description.", Image = "https://picsum.photos/800?image=8" });
            viewModel.Items.Add(new ItemViewModel { Title = "Tenth Item", Description = "Tenth Item short description.", Image = "https://picsum.photos/800?image=9" });


            BindingContext = viewModel;
        }
    }
}
