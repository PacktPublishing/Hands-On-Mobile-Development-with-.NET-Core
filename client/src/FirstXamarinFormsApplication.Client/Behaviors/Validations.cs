using System;
using System.Linq;
using FirstXamarinFormsApplication.Client.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;


[assembly: Dependency(typeof(Translator))]
namespace FirstXamarinFormsApplication.Client.Behaviors
{
    public interface ITranslateService
    {
        string Translate(string key);
    }

    public class TranslateService : ITranslateService
    {
        public string Translate(string key)
        {
            return "test";
        }
    }


    public class Translator : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            if(serviceType == typeof(ITranslateService))
            {
                return new TranslateService();
            }

            return null;
        }
    }

    [ContentProperty("Text")]
public class TranslateExtension : IMarkupExtension<string>
{
    public string Text { get; set; }

public string ProvideValue(IServiceProvider serviceProvider)
{

            var value = serviceProvider.GetService<ITranslateService>();

    switch (Text)
    {
        case "LblRequiredError":
            return "This a required field";
        case "LblUsername":
            return "Username";
        default:
            return Text;
    }
}

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
    }
}

    public interface IValidationRule<T>
    { 
        string ValidationMessage { get; set; }

        bool Validate (T value);
    }

    public class RequiredValidationRule : IValidationRule<string>
    {
        public string ValidationMessage { get; set; } = "This field is a required field";


        public bool Validate(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }

public static class Validations
{
    public static readonly BindableProperty ValidateRequiredProperty =
        BindableProperty.CreateAttached(
            "ValidateRequired",
            typeof(bool),
            typeof(Validations),
            false, 
            propertyChanged: OnValidateRequiredChanged);

    public static bool GetValidateRequired(BindableObject view)
    {
        return (bool)view.GetValue(ValidateRequiredProperty);
    }

    public static void SetValidateRequired(BindableObject view, bool value)
    {
        view.SetValue(ValidateRequiredProperty, value);
    }
    
private static void OnValidateRequiredChanged(BindableObject bindable, object oldValue, object newValue)
{
    if(bindable is Entry entry)
    {
        if ((bool)newValue)
        {
            entry.Behaviors.Add(new ValidationBehavior() { ValidationRule = new RequiredValidationRule() });
        }
        else
        {
            var behaviorToRemove = entry.Behaviors.OfType<ValidationBehavior>().FirstOrDefault(item => item.ValidationRule is RequiredValidationRule);

            if (behaviorToRemove != null)
            {
                entry.Behaviors.Remove(behaviorToRemove);
            }
        }
    }
}
}

    public class ValidationBehavior : Behavior<Entry>
{
    public static readonly BindableProperty ValidationRuleProperty =
        BindableProperty.CreateAttached("ValidationRule", typeof(IValidationRule<string>), typeof(ValidationBehavior), null);


    public static readonly BindableProperty HasErrorProperty =
        BindableProperty.CreateAttached("HasError", typeof(bool), typeof(ValidationBehavior), false, BindingMode.TwoWay);

    public IValidationRule<string> ValidationRule 
    {
        get { return this.GetValue(ValidationRuleProperty) as IValidationRule<string>; }
        set { this.SetValue(ValidationRuleProperty, value); }
    }

    public bool HasError
    {
        get { return (bool) GetValue(HasErrorProperty); }
        set { SetValue(HasErrorProperty, value); }
    }


    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.TextChanged += ValidateField;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        base.OnDetachingFrom(bindable);

        bindable.TextChanged -= ValidateField;
    }

private void ValidateField(object sender, TextChangedEventArgs args)
{
    if (sender is Entry entry && ValidationRule != null)
    {
        if (!ValidationRule.Validate(args.NewTextValue))
        {
            entry.BackgroundColor = Color.Crimson;

            HasError = true;
        }
        else
        {
            entry.BackgroundColor = Color.Transparent;
            HasError = false;
        }
    }
}
}
}
