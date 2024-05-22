using CSharpGraphsLibrary;
using GraphEditor.Models;
using Microsoft.Win32;
using System;
using System.Windows;
namespace GraphEditor.Commands
{
    class OpenGraphFromFileCommand : SyncCommand
    {
        public event Func<string, bool>? AttemptToOpenNamedGraph;
        public event Action<GraphInfo, Graph<int>>? GraphOpened;
        public event Action<GraphInfo, WeightedGraph<int, int>>? WeightedGraphOpened;
        public override void Execute(object? parameter)
        {
            OpenFileDialog dialog = new() { Filter = "XML files(.xml)|*.xml|all Files(*.*)|*.*" };
            if (dialog.ShowDialog() == true) // Returns 'bool?' so explicit comparison is required.
            {
                string filename  = dialog.FileName,
                       graphName = System.IO.Path.GetFileNameWithoutExtension(filename);
                if (AttemptToOpenNamedGraph?.Invoke(graphName) != true) // Returns 'bool?' so explicit comparison is required.
                {
                    MessageBox.Show("Graph name constructed from filename conflicts with existing graph name.",
                                    "Name conflict");
                    return;
                }
                try
                {
                    var graph = Graph<int>.DeserializeFromXML(filename);
                    GraphInfo graphInfo = new(graphName, GraphType.Unweighted);
                    GraphOpened?.Invoke(graphInfo, graph);
                }
                catch
                {
                    try // Nested 'try-catch' block required since deserialization methods throws exception on fail.
                    {
                        var weightedGraph = WeightedGraph<int, int>.DeserializeFromXML(filename);
                        GraphInfo graphInfo = new(graphName, GraphType.Weighted);
                        WeightedGraphOpened?.Invoke(graphInfo, weightedGraph);
                    }
                    catch
                    {
                        MessageBox.Show("Deserialization attempt failed. Probably file was corrupted and doesn't represent correct graph anymore.",
                                        "Deserialization fail");
                    }
                }
            }
        }
    }
}
