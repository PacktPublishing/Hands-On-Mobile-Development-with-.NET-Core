using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using FirstXamarinFormsApplication.iOS.Effects;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("FirstXamarinFormsApplication")]
[assembly: ExportEffect(typeof(HtmlTextEffect), "HtmlTextEffect")]
namespace FirstXamarinFormsApplication.iOS.Effects
{
public class HtmlTextEffect: PlatformEffect
{
    protected override void OnAttached()
    {
        SetHtmlText();
    }

    protected override void OnDetached()
    {
        // TODO: Remove formated text
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (args.PropertyName == Label.TextProperty.PropertyName)
        {
            SetHtmlText();
        }
    }

    private void SetHtmlText()
    {
        if (Control is UILabel label && Element is Label formLabel)
        {
            var documentAttributes = new NSAttributedStringDocumentAttributes();
            documentAttributes.DocumentType = NSDocumentType.HTML;
            var error = new NSError();

            label.AttributedText = new NSAttributedString(formLabel.Text, documentAttributes, ref error);
        }
    }
}
}
