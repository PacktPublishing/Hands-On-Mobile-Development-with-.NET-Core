using System;
using System.Linq;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client.Effects
{
public static class HtmlText
{
    public static readonly BindableProperty IsHtmlProperty =
        BindableProperty.CreateAttached("IsHtml", typeof(bool), typeof(HtmlText), false, propertyChanged: OnHtmlPropertyChanged);

    public static bool GetIsHtml(BindableObject view)
    {
        return (bool)view.GetValue(IsHtmlProperty);
    }

    public static void SetIsHtml(BindableObject view, bool value)
    {
        view.SetValue(IsHtmlProperty, value);
    }

    private static void OnHtmlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = bindable as View;
        if (view == null)
        {
            return;
        }

        if (newValue is bool isHtml && isHtml)
        {
            view.Effects.Add(new HtmlTextEffect());
        }
        else
        {
            var htmlEffect = view.Effects.FirstOrDefault(e => e is HtmlTextEffect);

            if (htmlEffect != null)
            {
                view.Effects.Remove(htmlEffect);
            }
        }
    }
}

public class HtmlTextEffect: RoutingEffect
{
    public HtmlTextEffect(): base("FirstXamarinFormsApplication.HtmlTextEffect")
    {
    }
}

    public class EnableHint: RoutingEffect
    {
        public EnableHint():base("FirstXamarinFormsApplication.EnableHint")
        {
        
}
    }
}
