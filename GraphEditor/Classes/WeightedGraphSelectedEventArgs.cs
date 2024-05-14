using CSharpGraphsLibrary;
using System;
namespace GraphEditor.Classes
{
    class WeightedGraphSelectedEventArgs : EventArgs
    {
        public string Name { get; }
        public WeightedGraph<int, int> WeightedGraph { get; }
        public WeightedGraphSelectedEventArgs(string name, WeightedGraph<int, int> weightedGraph)
        {
            Name = name;
            WeightedGraph = weightedGraph;
        }
    }
}
