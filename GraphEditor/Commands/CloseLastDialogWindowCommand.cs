namespace GraphEditor.Commands
{
    class CloseLastDialogWindowCommand : WindowManipulationCommand
    {
        public override void Execute(object? parameter) => OpenedWindowsStack.Peek().Close();
    }
}
