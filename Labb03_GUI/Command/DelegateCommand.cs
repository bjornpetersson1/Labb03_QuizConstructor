using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Labb03_GUI.Command
{
    class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute is null ? true : _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
}
