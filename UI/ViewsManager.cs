using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.ViewModels;
using WCFClient;

namespace UI
{

    public enum View { Login, Base };

    class ViewsManager : BindableBase
    {
        public static ViewsManager Instance;
        public WindowStyle WindowStyle { get => _windowStyle; set => SetProperty(ref _windowStyle, value); }
        public WindowState WindowState { get => _windowState; set => SetProperty(ref _windowState, value); }


        private BaseViewModel baseViewModel;
        private LoginViewModel loginViewModel = new LoginViewModel();
        private BindableBase _CurrentViewModel;
        private WindowState _windowState = WindowState.Normal;
        private WindowStyle _windowStyle = WindowStyle.SingleBorderWindow;

        public BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        public ViewsManager()
        {
            CurrentViewModel = loginViewModel;
            Instance = this;
        }


        public void FullMode()
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
        }
        public void NormalMode()
        {
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.SingleBorderWindow;
        }


        public void ChangeView(View view)
        {
            switch (view)
            {
                case View.Login:
                    CurrentViewModel = loginViewModel;
                    break;
                case View.Base:
                    CurrentViewModel = new BaseViewModel(this);
                    break;

                default:
                    throw new Exception("View model not found");
                    break;
            }
        }

        public void OnWindowsClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            API.proxy.Logout();
        }
    }
}
