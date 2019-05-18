using System.ComponentModel;
using System.Linq;
using Android.Text;
using Android.Widget;
using FirstXamarinFormsApplication.Android.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("FirstXamarinFormsApplication")]
[assembly: ExportEffect(typeof(HtmlTextEffect), "HtmlTextEffect")]
[assembly: ExportEffect(typeof(EnableHint), "EnableHint")]
namespace FirstXamarinFormsApplication.Android.Effects
{
    public class EnableHint: PlatformEffect
    {
        protected override void OnAttached()
        {
            if(Control is EditText editText && Element is Entry entry)
            {
                editText.Hint = entry.Placeholder;
            }
        }

        protected override void OnDetached()
        {
        }
    }

    public class HtmlTextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            SetHtmlText();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if(args.PropertyName == Label.TextProperty.PropertyName)
            {
                SetHtmlText();
            }
        }

        protected override void OnDetached()
        {
        }

        private void SetHtmlText()
        {
            if (Control is TextView label && Element is Label formsLabel)
            {
                label.SetText(Html.FromHtml(formsLabel.Text, FromHtmlOptions.ModeLegacy), TextView.BufferType.Spannable);
            }
        }
    }
}