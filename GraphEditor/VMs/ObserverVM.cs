using CSharpGraphsLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
namespace GraphEditor.VMs
{
    class ObserverVM : VMBase
    {
        public event Action<string>? GraphUpdated;
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
        public ObserverVM()
        {
            Name = "no graph";
            graphDisplay = new();
        }
        public void ReactGraphSet(string name, Graph<int>? graph)
        {
            Name = name;
            weightedGraph = null;
            this.graph = graph;
            if (graph is not null)
            {
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
            else
            {
                GraphDisplay.Clear();
                Vertices = 0.ToString();
                Edges = 0.ToString();
            }
        }
        public void ReactWeightedGraphSet(string name, WeightedGraph<int, int> weightedGraph)
        {
            Name = name;
            graph = null;
            this.weightedGraph = weightedGraph;
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
        public void DisplayOnVertexAdded(int vertex)
        {
            GraphDisplay.Add($"{vertex} : {"{  }"}");
            if (weightedGraph is null) Vertices = graph!.VertexCount.ToString();
            else Vertices = weightedGraph!.VertexCount.ToString();
            GraphUpdated?.Invoke(new(name!));
        }
        public void DisplayOnVertexRemoved(int vertex)
        {
            GraphUpdated?.Invoke(new(name!));
            int removedIndex = 0,
                    i = 0;
            string v = vertex.ToString();
            if (weightedGraph is null)
            {
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(v)) { removedIndex = i; break; }
                    else GraphDisplay[i] = GraphDisplay[i].Replace(v, string.Empty);
                }
                for (; i < GraphDisplay.Count; ++i)
                    GraphDisplay[i] = GraphDisplay[i].Replace(v, string.Empty);
                GraphDisplay.RemoveAt(removedIndex);
                Vertices = graph!.VertexCount.ToString();
                Edges = graph.EdgeCount.ToString();
            }
            else
            {
                string pattern = $"^({v} : [0-9]+)$";
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(v)) { removedIndex = i; break; }
                    else GraphDisplay[i] = Regex.Replace(GraphDisplay[i], pattern, string.Empty);
                }
                for (; i < GraphDisplay.Count; ++i)
                    GraphDisplay[i] = Regex.Replace(GraphDisplay[i], pattern, string.Empty);
                GraphDisplay.RemoveAt(removedIndex);
                Vertices = weightedGraph.VertexCount.ToString();
                Edges = weightedGraph.EdgeCount.ToString();
            }
        }
        public void DisplayOnEdgesChanged(int v1, int v2)
        {
            GraphUpdated?.Invoke(new(name!));
            int i = 0;
            bool firstFound = false;
            if (weightedGraph is null)
            {
                for (; i < GraphDisplay.Count; ++i)
                {
                    if (GraphDisplay[i].StartsWith(v1.ToString()))
                    {
                        GraphDisplay[i] = $"{v1} : {'{'} {string.Join(' ', graph!.NeighboursOf(v1)!)} {'}'}";
                        firstFound = true; break;
                    }
                    if (GraphDisplay[i].StartsWith(v2.ToString()))
                    {
                        GraphDisplay[i] = $"{v2} : {'{'} {string.Join(' ', graph!.NeighboursOf(v2)!)} {'}'}";
                        break;
                    }
                }
                if (firstFound)
                {
                    for (; i < GraphDisplay.Count; ++i)
                    {
                        if (GraphDisplay[i].StartsWith(v2.ToString()))
                        {
                            GraphDisplay[i] = $"{v2} : {'{'} {string.Join(' ', graph!.NeighboursOf(v2)!)} {'}'}";
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
                            GraphDisplay[i] = $"{v1} : {'{'} {string.Join(' ', graph!.NeighboursOf(v1)!)} {'}'}";
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
                        foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v1)!) SB.Append($"({n} : {w}) ");
                        GraphDisplay[i] = $"{v1} : {'{'} {SB}{'}'}";
                        SB.Clear();
                        firstFound = true; break;
                    }
                    if (GraphDisplay[i].StartsWith(v2.ToString()))
                    {
                        SB.Clear();
                        foreach ((int n, int w) in weightedGraph.NeighboursWithWeightOf(v2)!) SB.Append($"({n} : {w}) ");
                        GraphDisplay[i] = $"{v2} : {'{'} {SB}{'}'}";
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
    }
}
