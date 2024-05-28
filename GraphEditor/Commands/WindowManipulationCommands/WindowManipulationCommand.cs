using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
namespace GraphEditor.Commands.WindowManipulationCommands
{
    abstract class WindowManipulationCommand : SyncCommand
    {
        protected internal static Stack<Window> OpenedWindowsStack = new();
        protected static void OnWindowClose(object? sender, CancelEventArgs e) =>
            OpenedWindowsStack.Pop();
    }
}
