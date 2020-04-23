using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ViewModels;
namespace UI
{

    public enum View { Login, Base };

    class ViewsManager : BindableBase
    {
        public static ViewsManager Instance;

        private BaseViewModel baseViewModel;
        private LoginViewModel loginViewModel = new LoginViewModel();
        private BindableBase _CurrentViewModel;

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

     



        public void ChangeView(View view)
        {
            switch (view)
            {
                case View.Login:
                    CurrentViewModel = loginViewModel;
                    break;
                case View.Base:
                    CurrentViewModel = new BaseViewModel();
                    break;
                
                default:
                    throw new Exception("View model not found");
                    break;
            }
        }
    }
}
