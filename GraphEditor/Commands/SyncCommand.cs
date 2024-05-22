using System;
using System.Windows.Input;
namespace GraphEditor.Commands
{
    public abstract class SyncCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public abstract void Execute(object? parameter);
        public virtual bool CanExecute(object? parameter) => true;
        public virtual void NotifyCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
