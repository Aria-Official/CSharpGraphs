using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands.GraphActionCommands
{
    class RemoveVertexCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public event Action<int>? VertexRemoved;
        public RemoveVertexCommand(ActionsVM actionsVM) => this.actionsVM = actionsVM;
        public override void Execute(object? parameter)
        {
            try
            {
                bool graphNull = actionsVM.Graph is null,
                     weightedGraphNull = actionsVM.WeightedGraph is null;
                if (graphNull && weightedGraphNull)
                {
                    MessageBox.Show("No graph opened to perform actions on.", "No graph"); return;
                }
                string? v = actionsVM.Vertex;
                InputParser.ParseVertex(v, out int vertex, "Vertex was not specified.",
                                                           "Vertex doesn't parse to an integer.");
                if (weightedGraphNull)
                {
                    if (actionsVM.Graph!.TryRemoveVertex(vertex)) VertexRemoved?.Invoke(vertex);
                    else MessageBox.Show("Vertex was not in the graph.", "Vertex missing");
                }
                else
                {
                    if (actionsVM.WeightedGraph!.TryRemoveVertex(vertex)) VertexRemoved?.Invoke(vertex);
                    else MessageBox.Show("Vertex was not in the graph.", "Vertex missing");
                }
            }
            catch (InvalidInputException exc) { MessageBox.Show(exc.Message, "Input error"); }
        }
    }
}
