using CSharpGraphsLibrary;
using GraphEditor.Models.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
namespace GraphEditor.VMs
{
    class ObserverVM : VMBase
    {
        public event Action<NameEventArgs> GraphUpdated;
        Graph<int>? graph;
        WeightedGraph<int, int>? weightedGraph;
        readonly StringBuilder SB = new();
        string? vertices,
                edges,
                name;
        readonly ObservableCollection<string> graphDisplay;
        public string? Vertices
        {
            get => $"Vertices: {vertices}";
            set
            {
                vertices = value;
                NotifyPropertyChanged(nameof(Vertices));
            }
        }
        public string? Edges
        {
            get => $"Edges: {edges}";
            set
            {
                edges = value;
                NotifyPropertyChanged(nameof(Edges));
            }
        }
        public string? Name
        {
            get => $"Name: {name}";
            set
            {
                name = value!;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<string> GraphDisplay { get => graphDisplay; }
        public void ReactGraphSet(GraphEventArgs e)
        {
            Name = e.Name;
            weightedGraph = null;
            graph = e.Graph;
            Vertices = graph.VertexCount.ToString();
            Edges = graph.EdgeCount.ToString();
            // Display graph here.
            GraphDisplay.Clear();
            IEnumerable<int>? vs = graph.Vertices();
            if (vs is not null)
            {
                foreach (int v in vs)
                    GraphDisplay.Add($"{v} : {'{'} {string.Join(" ", graph.NeighboursOf(v)!)} {'}'}");
            }
        }
        public void ReactWeightedGraphSet(WeightedGraphEventArgs e)
        {
            Name = e.Name;
            graph = null;
            weightedGraph = e.WeightedGraph;
            Vertices = weightedGraph.VertexCount.ToString();
            Edges = weightedGraph.EdgeCount.ToString();
            // Display graph here.
            GraphDisplay.Clear();
            IEnumerable<int>? vs = weightedGraph.Vertices();
            if (vs is not null)
            {
                foreach (int v in vs)
                {
                    foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v)!) SB.Append($"({n} : {w})");
                    GraphDisplay.Add($"{v} : {'{'} {SB} {'}'}");
                    SB.Clear();
                }
            }
        }
        public void DisplayOnVertexAdded(VertexEventArgs e)
        {
            GraphDisplay.Add($"{e.Vertex} : {"{  }"}");
            if (weightedGraph is null) Vertices = graph!.VertexCount.ToString();
            else Vertices = weightedGraph!.VertexCount.ToString();
            GraphUpdated.Invoke(new(name!));
        }
        public void DisplayOnVertexRemoved(VertexEventArgs e)
        {
            GraphUpdated.Invoke(new(name!));
            int removedIndex = 0,
                    i = 0;
            string vertex = e.Vertex.ToString();
            if (weightedGraph is null)
            {
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(vertex)) { removedIndex = i; break; }
                    else GraphDisplay[i] = GraphDisplay[i].Replace(vertex, string.Empty);
                }
                for (; i < GraphDisplay.Count; ++i)
                    GraphDisplay[i] = GraphDisplay[i].Replace(vertex, string.Empty);
                GraphDisplay.RemoveAt(removedIndex);
                Vertices = graph!.VertexCount.ToString();
                Edges = graph.EdgeCount.ToString();
            }
            else
            {
                string pattern = $"^({vertex} : [0-9]+)$";
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(vertex)) { removedIndex = i; break; }
                    else GraphDisplay[i] = Regex.Replace(GraphDisplay[i], pattern, string.Empty);
                }
                for (; i < GraphDisplay.Count; ++i)
                    GraphDisplay[i] = Regex.Replace(GraphDisplay[i], pattern, string.Empty);
                GraphDisplay.RemoveAt(removedIndex);
                Vertices = weightedGraph.VertexCount.ToString();
                Edges = weightedGraph.EdgeCount.ToString();
            }
        }
        public void DisplayOnEdgesChanged(EdgeEventArgs e)
        {
            GraphUpdated.Invoke(new(name!));
            int v1 = e.EdgeStart,
                v2 = e.EdgeEnd,
                i = 0;
            bool firstFound = false;
            if (weightedGraph is null)
            {
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(v1.ToString()))
                    {
                        GraphDisplay[i] = $"{v1} : {'{'} {string.Join(" ", graph!.NeighboursOf(v1)!)} {'}'}";
                        firstFound = true; break;
                    }
                    if (GraphDisplay[i].StartsWith(v2.ToString()))
                    {
                        GraphDisplay[i] = $"{v2} : {'{'} {string.Join(" ", graph!.NeighboursOf(v2)!)} {'}'}";
                        break;
                    }
                }
                if (firstFound)
                {
                    for (; i < GraphDisplay.Count; ++i)
                    {
                        if (GraphDisplay[i].StartsWith(v2.ToString()))
                        {
                            GraphDisplay[i] = $"{v2} : {'{'} {string.Join(" ", graph!.NeighboursOf(v2)!)} {'}'}";
                            break;
                        }
                    }
                }
                else // secondFound.
                {
                    for (; i < GraphDisplay.Count; ++i)
                    {
                        if (GraphDisplay[i].StartsWith(v1.ToString()))
                        {
                            GraphDisplay[i] = $"{v1} : {'{'} {string.Join(" ", graph!.NeighboursOf(v1)!)} {'}'}";
                            break;
                        }
                    }
                }
                Edges = graph!.EdgeCount.ToString();
            }
            else
            {
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(v1.ToString()))
                    {
                        SB.Clear();
                        foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v1)!) SB.Append($"({n} : {w})");
                        GraphDisplay[i] = $"{v1} : {'{'} {SB} {'}'}";
                        SB.Clear();
                        firstFound = true; break;
                    }
                    if (GraphDisplay[i].StartsWith(v2.ToString()))
                    {
                        SB.Clear();
                        foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v2)!) SB.Append($"({n} : {w})");
                        GraphDisplay[i] = $"{v2} : {'{'} {SB} {'}'}";
                        SB.Clear();
                        break;
                    }
                }
                if (firstFound)
                {
                    for (; i < GraphDisplay.Count; ++i)
                    {
                        if (GraphDisplay[i].StartsWith(v2.ToString()))
                        {
                            SB.Clear();
                            foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v2)!) SB.Append($"({n} : {w})");
                            GraphDisplay[i] = $"{v2} : {'{'} {SB} {'}'}";
                            SB.Clear();
                            break;
                        }
                    }
                }
                else // secondFound.
                {
                    for (; i < GraphDisplay.Count; ++i)
                    {
                        if (GraphDisplay[i].StartsWith(v1.ToString()))
                        {
                            SB.Clear();
                            foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v1)!) SB.Append($"({n} : {w})");
                            GraphDisplay[i] = $"{v1} : {'{'} {SB} {'}'}";
                            SB.Clear();
                            break;
                        }
                    }
                }
                Edges = weightedGraph!.EdgeCount.ToString();
            }
        }
        public ObserverVM()
        {
            Name = "no graph";
            graphDisplay = new();
        }
    }
}
