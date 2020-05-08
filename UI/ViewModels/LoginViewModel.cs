using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Models;
using WCFClient;
using WCFClient.ServiceReference1;

namespace UI.ViewModels
{
    class LoginViewModel : BindableBase, INotifyDataErrorInfo
    {
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                this.errors.Clear(nameof(this.Username));
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                this.errors.Clear(nameof(this.Username));
                OnPropertyChanged(nameof(Password));
            }
        }
        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(nameof(PasswordError)); }
        }

        public IAsyncCommand LoginCommand { get; set; }
        public IAsyncCommand RegisterCommand { get; set; }

        public bool HasErrors => this.errors.HasErrors;
        ///private API api = API.GetInstance();
        private readonly Helpers.PropertyErrors errors;
        private string _username;
        private SecureString _password;
        private string _passwordError;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public LoginViewModel()
        {
            LoginCommand = new AsyncCommand(OnLogin);
            RegisterCommand = new AsyncCommand(OnRegister);
            errors = new Helpers.PropertyErrors(this, OnErrorsChanged);
        }


        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            this.ErrorsChanged?.Invoke(this, e);
        }

        private async Task OnLogin()
        {
            PasswordError = "";
            if (string.IsNullOrWhiteSpace(Username))
            {
                this.errors.Add(nameof(this.Username), "Username is required");
                return;
            }

            if (Password == null)
            {
                PasswordError = "Password cannot be empty!";
                return;
            }



            

            var res = await API.proxy.LoginAsync(Username, "pass");

            if (!res.HasError)
            {
                API.CurrentUser = res.Data;
                ViewsManager.Instance.ChangeView(View.Base);
            }
            else
            {
                switch (res.Error)
                {
                    case ResultError.NotFound:
                        errors.Add(nameof(Username), "Username not found!");
                        break;
                    case ResultError.PasswordIsIncorrect:
                        PasswordError = "Password is incorrect";
                        break;
                }
            }
        }

        private async Task OnRegister()
        {
            PasswordError = "";
            if (string.IsNullOrWhiteSpace(Username))
            {
                this.errors.Add(nameof(this.Username), "Username is required");
                return;
            }

            if (Password == null)
            {
                PasswordError = "Password is incorect!";
                return;
            }



            Result res = API.proxy.Register(Username, "pass");

            if (!res.HasError)
            {
                MessageBox.Show("Register success!");
            }
            else
            {
                switch (res.Error)
                {
                    case ResultError.UsernameIsUsed:
                        this.errors.Add(nameof(this.Username), "Username is used!");
                        break;
                    default:
                        PasswordError = "Server error.";
                        break;
                }
            }


        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.errors.GetErrors(propertyName);
        }
    }
}
