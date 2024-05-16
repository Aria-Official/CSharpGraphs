using System.Windows;

namespace GraphEditor.Commands.WindowManipulationCommands
{
    class OpenDialogWindowCommand<TWindow> : WindowManipulationCommand where TWindow : Window, new()
    {
        public override void Execute(object? parameter)
        {
            TWindow window = new();
            window.Closing += OnWindowClose;
            OpenedWindowsStack.Push(window);
            window.ShowDialog();
        }
    }
}
