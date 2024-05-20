using GraphEditor.Models.AlgorithmsTreeView;
using GraphEditor.VMs;
using System.Windows;
namespace GraphEditor.Commands
{
    class ExecuteAlgorithmCommand : SyncCommand
    {
        readonly AlgorithmsVM algorithmsVM;
        public ExecuteAlgorithmCommand(AlgorithmsVM algorithmsVM) => this.algorithmsVM = algorithmsVM;
        public override void Execute(object? parameter)
        {
            if (algorithmsVM.AlgorithmsTreeViewSelectedNode is AlgorithmsTreeViewItemAlgorithm alg)
            {
                alg.AlgorithmInvoker?.Invoke();
            }
            else MessageBox.Show("There wasn't selected algorithm to run.", "No algorithm selected");
        }
    }
}
