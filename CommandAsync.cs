using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyConvertor
{
    public class CommandAsync<T>: ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Predicate<T> _canExecute;
        private bool _isExecuting;

        public CommandAsync(Func<T,Task> execute):this(execute, null) { }

        public CommandAsync(Func<T, Task> execute, Predicate<T> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough()]
        public bool CanExecute(object param)
        {
            if (!_isExecuting && _canExecute == null) return true;
            return !_isExecuting && _canExecute((T)param);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="param">Command parameter</param>
        public async void Execute(object param)
        {
            _isExecuting = true;
            try
            {
                await _execute((T)param);
            }
            finally
            {
                _isExecuting = false;
            }
        }

    }
}
