namespace CSharpGraphsLibrary
{
    /// <summary>
    /// Provides container for graph algorithms that work with weight trees.
    /// </summary>
    public static class WeightTrees
    {
        /// <summary>
        /// Finds weight tree of highest or lowest cost represented as list of edges on the specifed disoriented strong connected
        /// weighted graph.
        /// </summary>
        /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
        /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
        /// <param name="graph">Disoriented strong connected weighted graph to build weight tree on.</param>
        /// <param name="root">Vertex to be root of weight tree.</param>
        /// <param name="comparison">Method to compare objects of graph edge weight.</param>
        /// <returns>List of edges of specified graph representing weight tree of highest or lowest cost with specified root vertex.</returns>
        /// <exception cref="ArgumentNullException">Is throw when:
        /// 1. specified graph is <see langword="null"/>;
        /// 2. specified comparison delegate is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">Is thrown when specified root vertex is not in the specified graph.</exception>
        public static List<(TVertex, TVertex, TEdgeWeight)> WeightTree<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph, TVertex root,
            Comparison<TEdgeWeight> comparison) where TVertex : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph} was null.'");
            if (!graph.HasVertex(root)) throw new InvalidOperationException(
                $"Specified vertex '{root}' was not in the graph.");
            if (comparison is null) throw new ArgumentNullException(
                $"Specified edge weight comparison function '{comparison}' was null.");
            PriorityQueue<(TVertex, TVertex), TEdgeWeight> queue = new(Comparer<TEdgeWeight>.Create(comparison));
            int joinedVertices = 1;
            List<(TVertex, TVertex, TEdgeWeight)> weightTree = new();
            foreach ((TVertex neighbour, TEdgeWeight weight) in graph.NeighboursWithWeightOf(root)!)
                queue.Enqueue((root, neighbour), weight);
            HashSet<TVertex> visited = new() { root };
            while (joinedVertices != graph.VertexCount && queue.Count > 0)
            {
                (TVertex v1, TVertex v2) = queue.Dequeue();
                if (visited.Contains(v1) && visited.Contains(v2)) continue;
                graph.HasEdge(v1, v2, false, out TEdgeWeight? edgeWeight);
                weightTree.Add((v1, v2, edgeWeight!));
                visited.Add(v2);
                ++joinedVertices;
                foreach ((TVertex neighbour, TEdgeWeight weight) in graph.NeighboursWithWeightOf(v2)!)
                    if (!visited.Contains(neighbour)) queue.Enqueue((v2, neighbour), weight);
            }
            return weightTree;
        }
    }
}
