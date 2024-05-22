using CSharpGraphsLibrary;
using GraphEditor.Commands;
using GraphEditor.Models;
using System;
using System.ComponentModel;
namespace GraphEditor.VMs
{
    class ExplorerVM : VMBase
    {
        readonly GraphHolder graphHolder;
        readonly BindingList<GraphInfo> graphInfos;
        GraphInfo? selectedGraphInfo;
        public event Action<string, Graph<int>?>? GraphSelected;
        public event Action<string, WeightedGraph<int, int>>? WeightedGraphSelected;
        public BindingList<GraphInfo> GraphInfos { get => graphInfos; }
        public GraphInfo? SelectedGraphInfo
        {
            get => selectedGraphInfo;
            set
            {
                selectedGraphInfo = value;
                NotifyPropertyChanged(nameof(SelectedGraphInfo));
                OnSelectedGraphInfoChanged(value);
            }
        }
        public SaveGraphCommand SaveGraphCommand { get; }
        public CloseGraphCommand CloseGraphCommand { get; }
        public ExplorerVM()
        {
            graphInfos = new();
            graphHolder = new();
            SaveGraphCommand = new(this);
            CloseGraphCommand = new(this);
        }
        void OnSelectedGraphInfoChanged(GraphInfo? graphInfo)
        {
            if (graphInfo is not null)
            {
                string name = graphInfo.Name;
                if (graphInfo.GraphType == GraphType.Unweighted)
                     GraphSelected?.Invoke(name, graphHolder.Graphs[name]);
                else WeightedGraphSelected?.Invoke(name, graphHolder.WeightedGraphs[name]);
            }
            else GraphSelected?.Invoke("no graph", null);
        }
        public bool VerifyAttemptToCreateNewGraph(GraphInfo graphInfo)
        {
            bool canAdd = VerifyGraphNameAllowed(graphInfo.Name);
            if (canAdd)
            {
                graphInfos.Add(graphInfo);
                if (graphInfo.GraphType == GraphType.Unweighted)
                     graphHolder.Graphs.Add(graphInfo.Name, Graph<int>.Create());
                else graphHolder.WeightedGraphs.Add(graphInfo.Name, WeightedGraph<int, int>.Create());
                SelectedGraphInfo = graphInfo;
            }
            return canAdd;
        }
        public void ReactGraphUpdated(string name)
        {
            for (int i = 0; i < GraphInfos.Count; ++i)
                if (GraphInfos[i].Name == name) { GraphInfos[i].Saved = false; break; }
        }
        public bool VerifyGraphNameAllowed(string graphName)
        {
            bool allow = true;
            foreach (GraphInfo graphInfo in graphInfos) 
                if (graphInfo.Name == graphName) { allow = false; break; }
            return allow;
        }
        public void ReactGraphOpened(GraphInfo graphInfo, Graph<int> graph)
        {
            graphInfo.Saved = true;
            graphInfos.Add(graphInfo);
            graphHolder.Graphs.Add(graphInfo.Name, graph);
            SelectedGraphInfo = graphInfo;
        }
        public void ReactWeightedGraphOpened(GraphInfo graphInfo, WeightedGraph<int, int> weightedGraph)
        {
            graphInfo.Saved = true;
            graphInfos.Add(graphInfo);
            graphHolder.WeightedGraphs.Add(graphInfo.Name, weightedGraph);
            SelectedGraphInfo = graphInfo;
        }
        public void ReactGraphClosed()
        {
            if (SelectedGraphInfo!.GraphType == GraphType.Unweighted)
                 graphHolder.Graphs.Remove(SelectedGraphInfo.Name);
            else graphHolder.WeightedGraphs.Remove(SelectedGraphInfo.Name);
            GraphInfos.Remove(SelectedGraphInfo!);
            if (graphInfos.Count > 0) SelectedGraphInfo = GraphInfos[0];
            else SelectedGraphInfo = null;
        }
        public Graph<int>? GetGraphByName(string name) =>
            graphHolder.Graphs.TryGetValue(name, out Graph<int>? graph) ? graph : null;
        public WeightedGraph<int, int>? GetWeightedGraphByName(string name) =>
            graphHolder.WeightedGraphs.TryGetValue(name, out WeightedGraph<int, int>? graph) ? graph : null;
    }
}
