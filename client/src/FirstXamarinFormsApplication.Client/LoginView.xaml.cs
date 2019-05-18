using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using FirstXamarinFormsApplication.Client;
using FirstXamarinFormsApplication.Client.Behaviors;
using FirstXamarinFormsApplication.Client.Diagnostics;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication
{
public partial class LoginView : ContentPage
{
    //private LoginViewController _controller;

    public LoginView()
    {
        InitializeComponent();

        //_controller = new LoginViewController(this);
        var loginViewModel = new LoginViewModel();

            BindingContext = loginViewModel;

            loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
            
    }

        //internal Entry UserName { get { return this.usernameEntry; }}

        //internal Entry Password { get { return this.passwordEntry; }}

        //internal Label Result { get { return this.messageLabel; } }

        //internal Button Login { get { return this.loginButton; }}

        //internal ToolbarItem SignUp { get { return this.signUpButton; }}

        void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Result")
            {
                var navigationPage = new MasterDetail();

                App.Current.MainPage = navigationPage;
            }
        }

    }

    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _userName = string.Empty;

        private string _password = string.Empty;

        private string _result = string.Empty;

        private Command _loginCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand;
            }
        }

        private IValidationRule<string> _usernameValidation;

        public IValidationRule<string> UserNameValidation
        {
            get
            {
                return _usernameValidation ?? (_usernameValidation = new RequiredValidationRule());
            }
        }

        public LoginViewModel()
        {
            _loginCommand = new Command(Login, Validate);
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    _loginCommand.ChangeCanExecute();
                }
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    _loginCommand.ChangeCanExecute();
                }
            }
        }

public string Result
{
    get
    {
        return _result;
    }
    set
    {
        if (_result != value)
        {
            _result = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result))); ;
        }
    }
}



public void Login()
{
    try
    {
        Result = "Successfully Logged In!";
                AppTelemetryRouter.Instance.TrackEvent(new LoginEvent() { Result = Result });

        //TODO: Login

    }
    catch(Exception ex)
    {
        MessagingCenter.Send(this, "ServiceError", ex.Message);
    }
}

        public bool Validate()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        public void Submit()
        {
            // TODO:
        }
    }

    public class LoginViewController
    {
        private LoginView _loginView;

        public LoginViewController(LoginView view)
        {
            _loginView = view;

            //_loginView.Login.Clicked += Login_Clicked;

           //_loginView.SignUp.Clicked += SignUp_Clicked;

           //_loginView.UserName.TextChanged += UserName_TextChanged;
        }

        void Login_Clicked(object sender, EventArgs e)
        {
            // TODO: Login
           //_loginView.Result.Text = "Successfully Logged In!";
        }

        void SignUp_Clicked(object sender, EventArgs e)
        {
            // TODO: Navigate to SignUp
        }

        void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Validate
        }
    }
}
