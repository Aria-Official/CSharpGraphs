using CSharpGraphsLibrary;
using System;
namespace GraphEditor.Models.CustomEventArgs
{
    class WeightedGraphEventArgs : EventArgs
    {
        public string Name { get; }
        public WeightedGraph<int, int> WeightedGraph { get; }
        public WeightedGraphEventArgs(string name, WeightedGraph<int, int> weightedGraph)
        {
            Name = name;
            WeightedGraph = weightedGraph;
        }
    }
}
