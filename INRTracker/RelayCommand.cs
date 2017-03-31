using System;
using System.Windows.Input;

namespace INRTracker
{
    /// <summary>
    ///     Implemenation of ICommand. Allows binding of Commands in ViewModel to Views.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Private Fields

        private readonly Action _targetExecuteMethod;
        private readonly Func<bool> _targetCanExecuteMethod;

        #endregion

        #region Public Fields

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion

        #region Methods

        public RelayCommand(Action executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
                return _targetCanExecuteMethod();
            if (_targetExecuteMethod != null)
                return true;
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            _targetExecuteMethod?.Invoke();
        }

        #endregion
    }

    /// <summary>
    /// Implemenation of ICommand supporting generics.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        #region Private Fields

        private readonly Action<T> _targetExecuteMethod;
        private readonly Func<T, bool> _targetCanExecuteMethod;

        #endregion

        #region Public Fields

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion

        #region Methods

        public RelayCommand(Action<T> executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
            {
                var tparm = (T) parameter;
                return _targetCanExecuteMethod(tparm);
            }
            if (_targetExecuteMethod != null)
                return true;
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            _targetExecuteMethod?.Invoke((T)parameter);
        }

        #endregion
    }
}
