using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImEditor
{
    class RelayCommand<T> : ICommand
    {
        private readonly Action<T> m_Execute;
        private readonly Predicate<T> m_CanExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return m_CanExecute?.Invoke((T)(parameter)) ?? true;
        }

        public void Execute(object parameter)
        {
            m_Execute((T)(parameter));
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_CanExecute = canExecute;
        }
    }
}
