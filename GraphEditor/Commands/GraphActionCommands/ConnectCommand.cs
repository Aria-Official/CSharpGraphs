using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.Models.CustomEventArgs;
using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands.GraphActionCommands
{
    class ConnectCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public event Action<EdgeEventArgs>? Connected;
        public ConnectCommand(ActionsVM actionsVM)
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
                bool oriented = actionsVM.Oriented;
                InputParser.ParseVertex(edgeStart, out int v1, "Edge start was not specified.",
                                                               "Edge start doesn't parse to an integer.");
                InputParser.ParseVertex(edgeEnd, out int v2, "Edge end was not specified.",
                                                             "Edge end doesn't parse to an integer.");
                EdgeEventArgs e = new(v1, v2);
                if (weightedGraphNull)
                {
                    actionsVM.Graph!.Connect(v1, v2, oriented);
                    Connected?.Invoke(e);
                }
                else
                {
                    string? weight = actionsVM.Weight;
                    InputParser.ParseWeight(weight, out int w, "Edge weight was not specified.",
                                                               "Edge weight doesn't parse to an integer.",
                                                               "Edge weight wasn't posisitive.");
                    actionsVM.WeightedGraph!.Connect(v1, v2, oriented, w);
                    Connected?.Invoke(e);
                }
            }
            catch (InvalidInputException IIExc) { MessageBox.Show(IIExc.Message, "Input error"); }
            catch (InvalidOperationException IOExc) { MessageBox.Show(IOExc.Message, "Vertex missing"); }
        }
    }
}
