using CSharpGraphsLibrary;
using GraphEditor.VMs;
using Microsoft.Win32;
namespace GraphEditor.Commands
{
    class SaveGraphCommand : SyncCommand
    {
        readonly ExplorerVM explorerVM;
        public SaveGraphCommand(ExplorerVM explorerVM) => this.explorerVM = explorerVM;
        public override void Execute(object? parameter)
        {
            string graphName = explorerVM.SelectedGraphInfo!.Name;
            SaveFileDialog saveFileDialog = new()
            {
                FileName = graphName,
                DefaultExt = ".xml",
                Filter = "XML files(.xml)|*.xml|all Files(*.*)|*.*",
                CreatePrompt = true,
                OverwritePrompt = true,
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = System.IO.Path.GetFullPath(saveFileDialog.FileName);
                Graph<int>? graph = explorerVM.GetGraphByName(graphName);
                if (graph is not null)
                {
                    Graph<int>.SerializeAsXML(graph, path);
                    explorerVM.SelectedGraphInfo.Saved = true;
                }
                else
                {
                    WeightedGraph<int, int> weightedGraph = explorerVM.GetWeightedGraphByName(graphName)!;
                    WeightedGraph<int, int>.SerializeAsXML(weightedGraph, path);
                    explorerVM.SelectedGraphInfo.Saved = true;
                }
            }
        }
    }
}
