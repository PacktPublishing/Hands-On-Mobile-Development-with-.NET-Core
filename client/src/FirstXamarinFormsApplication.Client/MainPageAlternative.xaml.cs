using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication
{
	public partial class MainPageAlternative : ContentPage
	{
		public MainPageAlternative()
		{
			InitializeComponent();

            BindingContext = new MainPageViewModel();
		}
	}
}
