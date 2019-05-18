using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using FirstXamarinFormsApplication.Client.Controls;
using FirstXamarinFormsApplication.Droid.Renderers;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FloatingLabelEntry), typeof(FloatingLabelEntryRenderer))]
namespace FirstXamarinFormsApplication.Droid.Renderers
{
    public class FloatingLabelEntryRenderer : ViewRenderer<FloatingLabelEntry, TextInputLayout>, ITextWatcher
{ 
    public FloatingLabelEntryRenderer(Context context) : base(context)
    {
    }

    /// <summary>
    /// Gets native edit text control.
    /// </summary>
    private EditText EditText => Control.EditText;

    /// <summary>
    /// Creates native text input layout control.
    /// </summary>
    /// <returns>Native text input layout control.</returns>
    protected override TextInputLayout CreateNativeControl()
    {
        var textInputLayout = new TextInputLayout(Context);
        var editText = new EditText(Context);
        editText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
        textInputLayout.AddView(editText);
        return textInputLayout;
    }

protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
    {
        Control.Hint = Element.Placeholder;
    }
    else if (e.PropertyName == FloatingLabelEntry.HasErrorProperty.PropertyName)
    {
        if (!Element.HasError || string.IsNullOrEmpty(Element.ErrorMessage))
        {
            EditText.Error = null;
            Control.ErrorEnabled = false;
        }
        else
        {
            Control.ErrorEnabled = true;
            EditText.Error = Element.ErrorMessage;
        }
    }
}

protected override void OnElementChanged(ElementChangedEventArgs<FloatingLabelEntry> e)
{
    base.OnElementChanged(e);

    if (e.OldElement == null)
    {
        var textView = CreateNativeControl();
                textView.EditText.AddTextChangedListener(this);
                textView.EditText.ImeOptions = ImeAction.Done;

                textView.Focusable = true;
                textView.HintEnabled = true;
                textView.HintAnimationEnabled = true;
                textView.EditText.ShowSoftInputOnFocus = true;

                SetNativeControl(textView);
    }

    Control.Hint = Element.Placeholder;
    EditText.Text = Element.Text;

    //EditText.Background.SetColorFilter(Element.AccentColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
}


        void ITextWatcher.AfterTextChanged(IEditable @string)
        {
        }

        void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }


        void ITextWatcher.OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (string.IsNullOrEmpty(Element.Text) && s.Length() == 0)
            {
                return;
            }

            ((IElementController)Element).SetValueFromRenderer(Entry.TextProperty, s.ToString());
        }
    }
}
