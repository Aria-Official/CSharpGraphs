using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands
{
    class CloseGraphCommand : SyncCommand
    {
        readonly ExplorerVM explorerVM;
        public event Action? GraphClosed;
        public CloseGraphCommand(ExplorerVM explorerVM) => this.explorerVM = explorerVM;
        public override void Execute(object? parameter)
        {
            if (!explorerVM.SelectedGraphInfo!.Saved)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Graph you are trying to close is not saved. Are you sure you want to close it?",
                    "Unsaved graph",
                    MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;
            }
            GraphClosed?.Invoke();
        }
    }
}
