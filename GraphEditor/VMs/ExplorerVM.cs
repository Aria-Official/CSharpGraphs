using CSharpGraphsLibrary;
using GraphEditor.Models;
using GraphEditor.Models.CustomEventArgs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GraphEditor.VMs
{
    class ExplorerVM : VMBase
    {
        readonly GraphHolder graphHolder;
        public event Action<GraphEventArgs> GraphSelected;
        public event Action<WeightedGraphEventArgs> WeightedGraphSelected;
        readonly BindingList<GraphInfo> graphInfos;
        public BindingList<GraphInfo> GraphInfos { get => graphInfos; }
        GraphInfo? selectedGraphInfo;
        public GraphInfo? SelectedGraphInfo
        {
            get => selectedGraphInfo;
            set
            {
                selectedGraphInfo = value;
                NotifyPropertyChanged(nameof(SelectedGraphInfo));
                string name = selectedGraphInfo!.Name;
                if (selectedGraphInfo!.GraphType == GraphType.Unweighted)
                    GraphSelected.Invoke(new(name, graphHolder.Graphs[name]));
                else WeightedGraphSelected.Invoke(new(name, graphHolder.WeightedGraphs[name]));
            }
        }
        public ExplorerVM()
        {
            graphInfos = new();
            graphHolder = new();
        }
        public bool VerifyAttemptToCreateNewGraph(GraphInfoEventArgs graphInfoEA)
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
        public void ReactGraphUpdated(NameEventArgs e)
        {
            for (int i = 0; i < GraphInfos.Count; ++i)
            {
                if (GraphInfos[i].Name == e.Name)
                {
                    GraphInfos[i].Saved = false;
                    break;
                }
            }
        }
    }
}
