﻿using GraphEditor.Exceptions;
using GraphEditor.Models;
using GraphEditor.Models.CustomEventArgs;
using GraphEditor.VMs;
using System;
using System.Windows;
namespace GraphEditor.Commands.GraphActionCommands
{
    class AddVertexCommand : SyncCommand
    {
        readonly ActionsVM actionsVM;
        public event Action<VertexEventArgs>? VertexAdded;
        public AddVertexCommand(ActionsVM actionsVM)
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
                    if (actionsVM.Graph!.TryAddVertex(vertex)) VertexAdded?.Invoke(new(vertex));
                    else MessageBox.Show("Vertex was already in the graph.", "Vertex exists");
                }
                else
                {
                    if (actionsVM.WeightedGraph!.TryAddVertex(vertex)) VertexAdded?.Invoke(new(vertex));
                    else MessageBox.Show("Vertex was already in the graph.", "Vertex exists");
                }
            }
            catch (InvalidInputException exc) { MessageBox.Show(exc.Message, "Input error"); }
        }
    }
}
