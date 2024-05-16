using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.VMs;
using System.Windows;
namespace GraphEditor.Commands.GraphActionCommands
{
    class HasVertexCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public HasVertexCommand(ActionsVM actionsVM)
        {
            this.actionsVM = actionsVM;
        }
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
                    if (actionsVM.Graph!.HasVertex(vertex)) MessageBox.Show("Vertex found.", "Found");
                    else MessageBox.Show("Vertex not found.", "Not found");
                }
                else
                {
                    if (actionsVM.WeightedGraph!.HasVertex(vertex)) MessageBox.Show("Vertex found.", "Not found");
                    else MessageBox.Show("Vertex not found.", "Found");
                }
            }
            catch (InvalidInputException exc) { MessageBox.Show(exc.Message, "Input error"); }
        }
    }
}
