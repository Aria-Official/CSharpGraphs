using GraphEditor.Classes;
using System.Collections.ObjectModel;
namespace GraphEditor.VMs
{
    class GraphExplorerVM : VMBase
    {
        readonly ObservableCollection<ExplorerGraphInfo> graphInfos;
        public ObservableCollection<ExplorerGraphInfo> GraphInfos { get => graphInfos; }
        public GraphExplorerVM()
        {
            graphInfos = new();
        }
    }
}
