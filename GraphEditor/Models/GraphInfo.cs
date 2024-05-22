using GraphEditor.VMs;
namespace GraphEditor.Models
{
    class GraphInfo : VMBase
    {
        bool saved;
        public string Name { get; init; }
        public bool Saved
        {
            get => saved;
            set
            {
                saved = value;
                NotifyPropertyChanged(nameof(Saved));
            }
        }
        public GraphType GraphType { get; init; }
        public GraphInfo(string name, GraphType graphType)
        {
            Name = name;
            Saved = false;
            GraphType = graphType;
        }
    }
}
