using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.VMs;
using System.Windows;
using System;
using GraphEditor.Models.CustomEventArgs;
namespace GraphEditor.Commands.GraphActionCommands
{
    class DisconnectCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public event Action<EdgeEventArgs>? Disconnected;
        public DisconnectCommand(ActionsVM actionsVM)
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
                string? edgeStart = actionsVM.EdgeStart,
                        edgeEnd = actionsVM.EdgeEnd;
                InputParser.ParseVertex(edgeStart, out int v1, "Edge start was not specified.",
                                                               "Edge start doesn't parse to an integer.");
                InputParser.ParseVertex(edgeEnd, out int v2, "Edge end was not specified.",
                                                             "Edge end doesn't parse to an integer.");
                EdgeEventArgs e = new(v1, v2);
                if (weightedGraphNull)
                {
                    actionsVM.Graph!.Disconnect(v1, v2);
                    Disconnected?.Invoke(e);
                }
                else
                {
                    actionsVM.WeightedGraph!.Disconnect(v1, v2);
                    Disconnected?.Invoke(e);
                }
            }
            catch (InvalidInputException IIExc) { MessageBox.Show(IIExc.Message, "Input error"); }
            catch (InvalidOperationException IOExc) { MessageBox.Show(IOExc.Message, "Vertex missing"); }
        }
    }
}
