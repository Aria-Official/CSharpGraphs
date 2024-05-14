using CSharpGraphsLibrary;
using GraphEditor.Classes;
using GraphEditor.Models;
using System.Collections.ObjectModel;
namespace GraphEditor.VMs
{
    class GraphExplorerVM : VMBase
    {
        readonly GraphHolder graphHolder;
        readonly ObservableCollection<GraphInfo> graphInfos;
        GraphInfo? selectedGraphInfo;
        public event GraphSelectedEventHandler GraphSelected;
        public event WeightedGraphSelectedEventHandler WeightedGraphSelected;
        public ObservableCollection<GraphInfo> GraphInfos { get => graphInfos; }
        public GraphInfo? SelectedGraphInfo
        {
            get => selectedGraphInfo;
            set
            {
                selectedGraphInfo = value;
                NotifyPropertyChanged(nameof(SelectedGraphInfo));
                string name = selectedGraphInfo!.Name;
                if (selectedGraphInfo!.GraphType == GraphType.Unweighted)
                    GraphSelected.Invoke(this, new(name, graphHolder.Graphs[name]));
                else WeightedGraphSelected.Invoke(this, new(name, graphHolder.WeightedGraphs[name]));
            }
        }
        public GraphExplorerVM()
        {
            graphInfos = new();
            graphHolder = new GraphHolder();
        }
        public bool VerifyAttemptToCreateNewGraph(object? sender, GraphInfoEventArgs graphInfoEA)
        {
            GraphInfo newGraphInfo = graphInfoEA.GraphInfo;
            bool canAdd = true;
            foreach (GraphInfo containedGraphInfo in graphInfos)
            {
                if (containedGraphInfo.Name == newGraphInfo.Name) { canAdd = false; break; }
            }
            if (canAdd)
            {
                graphInfos.Add(newGraphInfo);
                if (newGraphInfo.GraphType == GraphType.Unweighted)
                    graphHolder.Graphs.Add(newGraphInfo.Name, Graph<int>.Create());
                else graphHolder.WeightedGraphs.Add(newGraphInfo.Name, WeightedGraph<int, int>.Create());
                SelectedGraphInfo = newGraphInfo;
            }
            return canAdd;
        }
    }
}
