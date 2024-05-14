using CSharpGraphsLibrary;
using System;
namespace GraphEditor.Classes
{
    class GraphSelectedEventArgs : EventArgs
    {
        public string Name { get; }
        public Graph<int> Graph { get; }
        public GraphSelectedEventArgs(string name, Graph<int> graph)
        {
            Name = name;
            Graph = graph;
        }
    }
}
