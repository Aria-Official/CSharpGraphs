namespace CSharpGraphsLibrary
{
    /// <summary>
    /// Provides container for graph traversal algorithms.
    /// </summary>
    public static class Traversals
    {
        /// <summary>
        /// Traverses specified graph using depth first search (DFS) starting with specified start vertex.
        /// </summary>
        /// <typeparam name="T">Type of graph vertex.</typeparam>
        /// <param name="graph">Graph to be traversed.</param>
        /// <param name="start">Vertex to start traversal from.</param>
        /// <returns>Enumerable sequence of vertices representing result of depth first traversal (DFT).</returns>
        /// <exception cref="ArgumentNullException">Is thrown when specified graph is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when specified start vertex is not in the specified graph.</exception>
        public static IEnumerable<T> DepthFirstTraversal<T>(ITraversableGraph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException($"Specified vertex '{start}' was not in the graph.");
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
        /// <summary>
        /// Traverses specified graph using breadth first search (BFS) starting with specified start vertex.
        /// </summary>
        /// <typeparam name="T">Type of graph vertex.</typeparam>
        /// <param name="graph">Graph to be traversed.</param>
        /// <param name="start">Vertex to start traversal from.</param>
        /// <returns>Enumerable sequence of vertices representing result of breadth first traversal (BFT).</returns>
        /// <exception cref="ArgumentNullException">Is thrown when specified graph is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when specified start vertex is not in the specified graph.</exception>
        public static IEnumerable<T> BreadthFirstTraversal<T>(ITraversableGraph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException($"Specified vertex '{start}' was not in the graph.");
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
    }
}
