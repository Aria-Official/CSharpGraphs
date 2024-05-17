using CSharpGraphsLibrary;
using System.Collections.Generic;
namespace GraphEditor.Models
{
    class GraphHolder
    {
        public Dictionary<string, Graph<int>> Graphs { get; }
        public Dictionary<string, WeightedGraph<int, int>> WeightedGraphs { get; }
        public GraphHolder() { Graphs = new(); WeightedGraphs = new(); }
    }
}
