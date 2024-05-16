using GraphEditor.Models;
using GraphEditor.Models.CustomEventArgs;
using GraphEditor.Stores;
using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands
{
    class TryCreateNewGraphCommand : SyncCommand
    {
        public event Func<GraphInfoEventArgs, bool> AttemptToCreateNewGraph;
        readonly NewGraphPromptVM newGraphPromptViewModel;
        public TryCreateNewGraphCommand(NewGraphPromptVM newGraphPromptViewModel)
        {
            this.newGraphPromptViewModel = newGraphPromptViewModel;
            AttemptToCreateNewGraph += MainVMStore.MainVM.ExplorerVM.VerifyAttemptToCreateNewGraph;
        }
        public override void Execute(object? parameter)
        {
            string? graphName = newGraphPromptViewModel.GraphName;
            bool weighted = newGraphPromptViewModel.Weighted;
            if (graphName is null || graphName == string.Empty)
            {
                MessageBox.Show("Graph name was not specified. Specify graph name.", "Empty graph name");
                return;
            }
            GraphInfo graphInfo = new(graphName, weighted ? GraphType.Weighted : GraphType.Unweighted);
            bool canAdd = AttemptToCreateNewGraph!.Invoke(new(graphInfo));
            if (!canAdd) MessageBox.Show("Graph with specified name already exists. Specify other name.", "Existing graph name");
        }
    }
}
