using GraphEditor.Models;
using GraphEditor.Stores;
using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands
{
    class TryCreateNewGraphCommand : SyncCommand
    {
        readonly NewGraphPromptVM newGraphPromptVM;
        public event Func<GraphInfo, bool>? AttemptToCreateNewGraph;
        public TryCreateNewGraphCommand(NewGraphPromptVM newGraphPromptVM)
        {
            this.newGraphPromptVM = newGraphPromptVM;
            AttemptToCreateNewGraph += MainVMStore.MainVM.ExplorerVM.VerifyAttemptToCreateNewGraph;
        }
        public override void Execute(object? parameter)
        {
            string? graphName = newGraphPromptVM.GraphName;
            bool weighted = newGraphPromptVM.Weighted;
            if (graphName is null || graphName == string.Empty)
            {
                MessageBox.Show("Graph name was not specified. Specify graph name.",
                                "Empty graph name");
                return;
            }
            GraphInfo graphInfo = new(graphName, weighted ? GraphType.Weighted : GraphType.Unweighted);
            bool? canAdd = AttemptToCreateNewGraph?.Invoke(graphInfo);
            if (canAdd != true) // Returns 'bool?' so explicit comparison is required.
                MessageBox.Show("Graph with specified name already exists. Specify other name.",
                                "Existing graph name");
        }
    }
}
