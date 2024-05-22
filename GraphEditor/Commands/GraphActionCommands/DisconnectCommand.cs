using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.VMs;
using System.Windows;
using System;
namespace GraphEditor.Commands.GraphActionCommands
{
    class DisconnectCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public event Action<int, int>? Disconnected;
        public DisconnectCommand(ActionsVM actionsVM) => this.actionsVM = actionsVM;
        public override void Execute(object? parameter)
        {
            try
            {
                bool graphNull         = actionsVM.Graph is null,
                     weightedGraphNull = actionsVM.WeightedGraph is null;
                if (graphNull && weightedGraphNull)
                {
                    MessageBox.Show("No graph opened to perform actions on.", "No graph");
                    return;
                }
                string? edgeStart = actionsVM.EdgeStart,
                        edgeEnd   = actionsVM.EdgeEnd;
                InputParser.ParseVertex(edgeStart, out int v1, "Edge start was not specified.",
                                                               "Edge start doesn't parse to an integer.");
                InputParser.ParseVertex(edgeEnd, out int v2, "Edge end was not specified.",
                                                             "Edge end doesn't parse to an integer.");
                if (weightedGraphNull)
                {
                    actionsVM.Graph!.Disconnect(v1, v2);
                    Disconnected?.Invoke(v1, v2);
                }
                else
                {
                    actionsVM.WeightedGraph!.Disconnect(v1, v2);
                    Disconnected?.Invoke(v1, v2);
                }
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Vertex missing"); }
        }
    }
}
