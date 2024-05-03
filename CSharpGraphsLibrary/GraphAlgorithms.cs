namespace CSharpGraphsLibrary
{
    public static class GraphAlgorithms
    {
        public static IEnumerable<T> DepthFirstTraversal<T>(Graph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException(
                $"DepthFirstTraversal() failed. Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException(
                $"DepthFirstTraversal() failed. Specified graph '{graph}' had no vertices.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException(
                $"DepthFirstTraversal() failed. Specified vertex '{start}' was not in the graph.");
            HashSet<T> visited = new();
            DFT(start);
            foreach (T vertex in graph.Vertices()!)
            {
                if (!visited.Contains(vertex)) DFT(vertex);
            }
            return visited;
            void DFT(T current)
            {
                visited.Add(current);
                foreach (T neighbour in graph.NeighboursOf(current)!)
                {
                    if (!visited.Contains(neighbour)) DFT(neighbour);
                }
            }
        }
        public static IEnumerable<T> BreadthFirstTraversal<T>(Graph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException(
                $"BreadthFirstTraversal() failed. Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException(
                $"DepthFirstTraversal() failed. Specified graph '{graph}' had no vertices.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException(
                $"BreadthFirstTraversal() failed. Specified vertex '{start}' was not in the graph.");
            HashSet<T> visited = new(),
                       front = new() { start },
                       newFront = new();
            BFT();
            foreach (T vertex in graph.Vertices()!)
            {
                if (!visited.Contains(vertex)) { front.Add(vertex); BFT(); }
            }
            return visited;
            void BFT()
            {
                while (front.Count != 0)
                {
                    visited.UnionWith(front);
                    newFront.Clear();
                    foreach (T vertex in front)
                    {
                        foreach (T neighbour in graph.NeighboursOf(vertex)!) newFront.Add(neighbour);
                    }
                    front = newFront;
                }
            }
        }
        public static (Dictionary<T, int>, Dictionary<T, T>) Dijkstra<T>(Graph<T> graph, T start) where T : notnull
        {
            throw new NotImplementedException();
        }
    }
}
