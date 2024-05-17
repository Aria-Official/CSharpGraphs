using CSharpGraphsLibrary;
using GraphEditor.Commands.GraphActionCommands;
namespace GraphEditor.VMs
{
    class ActionsVM : VMBase
    {
        public Graph<int>? Graph { get; private set; }
        public WeightedGraph<int, int>? WeightedGraph { get; private set; }
        string? vertex,
                edgeStart,
                edgeEnd,
                weight;
        bool oriented,
             weightOptionVisibility;
        public string? Vertex
        {
            get => vertex;
            set
            {
                vertex = value;
                NotifyPropertyChanged(nameof(Vertex));
            }
        }
        public string? EdgeStart
        {
            get => edgeStart;
            set
            {
                edgeStart = value;
                NotifyPropertyChanged(nameof(EdgeStart));
            }
        }
        public string? EdgeEnd
        {
            get => edgeEnd;
            set
            {
                edgeEnd = value;
                NotifyPropertyChanged(nameof(EdgeEnd));
            }
        }
        public string? Weight
        {
            get => weight;
            set
            {
                weight = value;
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
        public bool WeightOptionVisibility
        {
            get => weightOptionVisibility;
            set
            {
                weightOptionVisibility = value;
                NotifyPropertyChanged(nameof(WeightOptionVisibility));
            }
        }
        public AddVertexCommand AddVertexCommand { get; }
        public RemoveVertexCommand RemoveVertexCommand { get; }
        public HasVertexCommand HasVertexCommand { get; }
        public ConnectCommand ConnectCommand { get; }
        public DisconnectCommand DisconnectCommand { get; }
        public HasEdgeCommand HasEdgeCommand { get; }
        public void ReactGraphSet(string _, Graph<int>? graph)
        {
            WeightedGraph = null;
            Graph = graph;
            WeightOptionVisibility = false;
            Weight = null;
        }
        public void ReactWeightedGraphSet(string _, WeightedGraph<int, int> weightedGraph)
        {
            Graph = null;
            WeightedGraph = weightedGraph;
            WeightOptionVisibility = true;
        }
        public ActionsVM()
        {
            AddVertexCommand = new(this);
            RemoveVertexCommand = new(this);
            HasVertexCommand = new(this);
            ConnectCommand = new(this);
            DisconnectCommand = new(this);
            HasEdgeCommand = new(this);
        }
    }
}
