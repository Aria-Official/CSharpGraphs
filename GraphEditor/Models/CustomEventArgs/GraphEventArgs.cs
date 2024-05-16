using CSharpGraphsLibrary;
using System;
namespace GraphEditor.Models.CustomEventArgs
{
    class GraphEventArgs : EventArgs
    {
        public string Name { get; }
        public Graph<int> Graph { get; }
        public GraphEventArgs(string name, Graph<int> graph)
        {
            Name = name;
            Graph = graph;
        }
    }
}
