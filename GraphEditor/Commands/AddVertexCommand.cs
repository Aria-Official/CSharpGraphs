using GraphEditor.VMs;
using System.Windows;
namespace GraphEditor.Commands
{
    class AddVertexCommand : SyncCommand
    {
        readonly GraphOptionsVM graphOptionsVM;
        public AddVertexCommand(GraphOptionsVM graphOptionsVM)
        {
            this.graphOptionsVM = graphOptionsVM;
        }
        public override void Execute(object? parameter)
        {
            string? v = graphOptionsVM.Vertex;
            if (v is null || v == string.Empty)
            { MessageBox.Show("Vertex was not specified.", "No vertex"); return; }
            bool parsed = int.TryParse(graphOptionsVM.Vertex, out int vertex);
            if (!parsed)
            {
                MessageBox.Show("Vertex doesn't parse to an integer.",
                                "Vertex doesn't parse");
                return;
            }
            if (graphOptionsVM.WeightedGraph is null)
            {
                if (graphOptionsVM.Graph!.TryAddVertex(vertex))
                {
                    // Invoke event to change draw.
                }
                else MessageBox.Show("Vertex was already in the graph.", "Vertex exists");
            }
            else
            {
                if (graphOptionsVM.WeightedGraph.TryAddVertex(vertex))
                {
                    // Invoke event to change draw.
                }
                else MessageBox.Show("Vertex was already in the graph.", "Vertex exists");
            }
        }
    }
}
