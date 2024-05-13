using System;
using System.Collections.ObjectModel;
namespace GraphEditor.VMs
{
    class GraphObserverVM : VMBase
    {
        int vertices,
            edges;
        string? name;
        ObservableCollection<string> graphDisplay;
        public string? Vertices
        {
            get => $"Vertices: {vertices}";
            set
            {
                vertices = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(Vertices));
            }
        }
        public string? Edges
        {
            get => $"Edges: {edges}";
            set
            {
                edges = Convert.ToInt32(value);
                NotifyPropertyChanged(nameof(Edges));
            }
        }
        public string? Name
        {
            get => name;
            set
            {
                name = value!;
                NotifyPropertyChanged(nameof(Name));
            }
        }
        public ObservableCollection<string> GraphDisplay { get => graphDisplay; }
        public GraphObserverVM()
        {
            Name = "No graph";
        }
    }
}
