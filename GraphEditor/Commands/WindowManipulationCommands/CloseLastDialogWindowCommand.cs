namespace GraphEditor.Commands.WindowManipulationCommands
{
    class CloseLastDialogWindowCommand : WindowManipulationCommand
    {
        public override void Execute(object? parameter) => OpenedWindowsStack.Peek().Close();
    }
}
