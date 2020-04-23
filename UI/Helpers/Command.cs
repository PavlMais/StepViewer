using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UI
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> _TargetExecuteMethod;
        private Func<object, bool> _TargetCanExecuteMethod;


        public Command(Action<object> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public Command(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                object tparm = (object)parameter;
                return _TargetCanExecuteMethod(tparm);
            }

            if (_TargetExecuteMethod != null)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            _TargetExecuteMethod?.Invoke(parameter);
        }
    }
}
