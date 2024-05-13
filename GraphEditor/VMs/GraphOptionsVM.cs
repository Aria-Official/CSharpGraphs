﻿using System;
using System.Windows.Input;
namespace GraphEditor.VMs
{
    class GraphOptionsVM : VMBase
    {
        int vertex,
            edgeEnd1,
            edgeEnd2,
            weight;
        bool oriented;
        public string? Vertex
        {
            get => vertex.ToString();
            set
            {
                vertex = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(Vertex));
            }
        }
        public string? EdgeEnd1
        {
            get => edgeEnd1.ToString();
            set
            {
                edgeEnd1 = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(EdgeEnd1));
            }
        }
        public string? EdgeEnd2
        {
            get => edgeEnd2.ToString();
            set
            {
                edgeEnd2 = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(EdgeEnd2));
            }
        }
        public string? Weight
        {
            get => weight.ToString();
            set
            {
                weight = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(Weight));
            }
        }
        public bool Oriented
        {
            get => oriented;
            set
            {
                oriented = value;
                NotifyPropertyChanged(nameof(Oriented));
            }
        }
        public ICommand AddVertexCommand { get; }
        public ICommand RemoveVertexCommand { get; }
        public ICommand HasVertexCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand HasEdgeCommand { get; }
        public GraphOptionsVM(ICommand addVertexCommand,
                              ICommand removeVertexCommand,
                              ICommand hasVertexCommand,
                              ICommand connectCommand,
                              ICommand disconnectCommand,
                              ICommand hasEdgeCommand)
        {
            AddVertexCommand = addVertexCommand;
            RemoveVertexCommand = removeVertexCommand;
            HasVertexCommand = hasVertexCommand;
            ConnectCommand = connectCommand;
            DisconnectCommand = disconnectCommand;
            HasEdgeCommand = hasEdgeCommand;
        }
    }
}
