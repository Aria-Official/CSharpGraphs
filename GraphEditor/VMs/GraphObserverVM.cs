using CSharpGraphsLibrary;
using GraphEditor.Classes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace GraphEditor.VMs
{
    class GraphObserverVM : VMBase
    {
        Graph<int>? graph;
        WeightedGraph<int, int>? weightedGraph;

        int vertices,
            edges;
        string? name;
        readonly ObservableCollection<string> graphDisplay;
        public string? Vertices { get => $"Vertices: {vertices}"; }
        void SetVertices(int v) { vertices = v; NotifyPropertyChanged(Vertices); }
        public string? Edges { get => $"Edges: {edges}"; }
        void SetEdges(int e) { edges = e; NotifyPropertyChanged(Edges); }
        public string? Name
        {
            get => name;
            set
            {
                name = value!;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<string> GraphDisplay { get => graphDisplay; }
        public void ReactGraphSet(object? sender, GraphSelectedEventArgs e)
        {
            Name = e.Name;
            weightedGraph = null;
            graph = e.Graph;
            SetVertices(graph.VertexCount);
            SetEdges(graph.EdgeCount);
            RedrawGraph(graph);
        }
        public void ReactWeightedGraphSet(object? sender, WeightedGraphSelectedEventArgs e)
        {
            Name = e.Name;
            graph = null;
            weightedGraph = e.WeightedGraph;
            SetVertices(weightedGraph.VertexCount);
            SetEdges(weightedGraph.EdgeCount);
            RedrawGraph(weightedGraph);
        }
        void RedrawGraph(ITraversableGraph<int> graph)
        {
            GraphDisplay.Clear();
            IEnumerable<int>? vs = graph.Vertices();
            if (vs is not null)
            {
                foreach (int v in vs) GraphDisplay.Add($"{v} : {string.Join(" ,", graph.NeighboursOf(v)!)}");
            }
        }
        public GraphObserverVM()
        {
            Name = "No graph";
            graphDisplay = new();
        }
    }
}
