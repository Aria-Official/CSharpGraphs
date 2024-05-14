using System;
using System.Windows.Input;
namespace GraphEditor.Commands
{
    public abstract class SyncCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object? parameter) => true;
        public abstract void Execute(object? parameter);
    }
}
