using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.VMs;
using System.Windows;
using System;
namespace GraphEditor.Commands.GraphActionCommands
{
    class HasEdgeCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public HasEdgeCommand(ActionsVM actionsVM) => this.actionsVM = actionsVM;
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
                string? edgeStart = actionsVM.EdgeStart,
                        edgeEnd = actionsVM.EdgeEnd;
                bool oriented = actionsVM.Oriented;
                InputParser.ParseVertex(edgeStart, out int v1, "Edge start was not specified.",
                                                               "Edge start doesn't parse to an integer.");
                InputParser.ParseVertex(edgeEnd, out int v2, "Edge end was not specified.",
                                                             "Edge end doesn't parse to an integer.");
                if (weightedGraphNull)
                {
                    if (actionsVM.Graph!.HasEdge(v1, v2, oriented)) MessageBox.Show("Edge found.", "Found");
                    else MessageBox.Show("Edge not found.", "Not found");
                }
                else
                {
                    if (actionsVM.WeightedGraph!.HasEdge(v1, v2, oriented)) MessageBox.Show("Edge found.", "Found");
                    else MessageBox.Show("Edge not found.", "Not found");
                }
            }
            catch (InvalidInputException exc) { MessageBox.Show(exc.Message, "Input error"); }
        }
    }
}
