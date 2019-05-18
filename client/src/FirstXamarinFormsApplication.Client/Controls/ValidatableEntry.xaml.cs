using System;
using System.Collections.Generic;
using FirstXamarinFormsApplication.Client.Behaviors;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client.Controls
{
public class FloatingLabelEntry : Entry
{
    public static readonly BindableProperty ErrorMessageProperty =
        BindableProperty.CreateAttached("ErrorMessage", typeof(string), typeof(FloatingLabelEntry), string.Empty);

    public static readonly BindableProperty HasErrorProperty =
BindableProperty.CreateAttached("HasError", typeof(bool), typeof(FloatingLabelEntry), false);

        public string ErrorMessage
    {
        get
        {
            return (string)GetValue(ErrorMessageProperty);
        }
        set
        {
            SetValue(ErrorMessageProperty, value);
        }
    }

    public bool HasError
    {
        get
        {
            return (bool)GetValue(HasErrorProperty);
        }
        set
        {
            SetValue(HasErrorProperty, value);
        }
    }
    }

    public partial class ValidatableEntry : ContentView
    {
        public static readonly BindableProperty LabelProperty =
            BindableProperty.CreateAttached("Label", typeof(string), typeof(ValidatableEntry), string.Empty);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.CreateAttached("Placeholder", typeof(string), typeof(ValidatableEntry), string.Empty);

        public static readonly BindableProperty ValueProperty =
            BindableProperty.CreateAttached("Value", typeof(string), typeof(ValidatableEntry), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty ValidationRuleProperty =
            BindableProperty.CreateAttached("ValidationRule", typeof(IValidationRule<string>), typeof(ValidationBehavior), null);

        public ValidatableEntry()
        {
            if (DesignMode.IsDesignModeEnabled)
            {
                Placeholder = "Here is the placeholder";
                Label = "Test Label";
            }

            InitializeComponent();

        }

        public IValidationRule<string> ValidationRule
        {
            get
            {
                return (IValidationRule<string>)GetValue(ValidationRuleProperty);
            }

            set
            {
                SetValue(ValidationRuleProperty, value);
            }
        }

        public string Placeholder 
        {
            get 
            {
                return (string)GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            } 
        }

        public string Label
        {
            get
            {
                return (string)GetValue(LabelProperty);
            }
            set
            {
                SetValue(LabelProperty, value);
            }
        }

        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }
    }
}
