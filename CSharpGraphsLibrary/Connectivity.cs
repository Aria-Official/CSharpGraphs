namespace CSharpGraphsLibrary
{
    /// <summary>
    /// Provides container for algorithms that are associated with connections in the graph.
    /// </summary>
    public static class Connectivity
    {
        /// <summary>
        /// Finds strong connected components as a list of sets of vertices of the specified graph.
        /// </summary>
        /// <typeparam name="T">Type of graph vertex.</typeparam>
        /// <param name="graph">Graph to find strong connected components of.</param>
        /// <returns>List of sets of vertices representing strong connected components of the specified graph.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when specified graph is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when specified graph has no vertices.</exception>
        public static List<HashSet<T>> StrongConnectedComponents<T>(ITraversableGraph<T> graph) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
            HashSet<T> inStack = new();
            List<HashSet<T>> components = new();
            Dictionary<T, int> onStep = new(graph.VertexCount);
            Dictionary<T, int> rootStep = new(graph.VertexCount);
            IEnumerable<T> vertices = graph.Vertices()!;
            Stack<T> stack = new();
            foreach (T vertex in graph.Vertices()!)
            {
                if (onStep.ContainsKey(vertex)) continue;
                OpenVertex(vertex);
            }
            return components;
            void OpenVertex(T vertex)
            {
                rootStep[vertex] = onStep.Count;
                onStep[vertex] = onStep.Count;
                stack.Push(vertex);
                inStack.Add(vertex);
                foreach (T neighbour in graph.NeighboursOf(vertex)!)
                {
                    if (!onStep.ContainsKey(neighbour))
                    {
                        OpenVertex(neighbour);
                        rootStep[vertex] = Math.Min(rootStep[vertex], rootStep[neighbour]);
                    }
                    else if (inStack.Contains(neighbour)) rootStep[vertex] = rootStep[neighbour];
                }
                int root = rootStep[vertex];
                if (root == onStep[vertex])
                {
                    HashSet<T> component = new();
                    while (stack.Count > 0 && rootStep[stack.Peek()] == root)
                    {
                        T popped = stack.Pop();
                        component.Add(popped);
                        inStack.Remove(popped);
                    }
                    components.Add(component);
                }
            }
        }
        /// <summary>
        /// Checks if specified graph is fully strong connected comparing amount of strong connected components with 1.
        /// </summary>
        /// <typeparam name="T">Type of graph vertex.</typeparam>
        /// <param name="graph">Graph to check strong connection of all vertices of.</param>
        /// <returns><see langword="true"/> if graph is fully strong connected; otherwise, <see langword="false"/>.</returns>
        public static bool IsStrongConnected<T>(ITraversableGraph<T> graph) where T : notnull
            => StrongConnectedComponents(graph).Count == 1;
        /// <summary>
        /// Creates subgraphs built on strong connected components of specified graph.
        /// </summary>
        /// <typeparam name="T">Type of graph vertex.</typeparam>
        /// <param name="graph">Graph to build subgraphs built on strong connected components of.</param>
        /// <returns>List of subgraphs built on strong connected components of specified graph.</returns>
        public static List<Graph<T>> CreateSubgraphsFromStrongConnectedComponents<T>(
            Graph<T> graph) where T : notnull
        {
            var components = StrongConnectedComponents(graph);
            List<Graph<T>> subgraphs = new();
            foreach (HashSet<T> component in components) subgraphs.Add(Graph<T>.Create(component));
            var edges = graph.Edges();
            if (edges is null) return subgraphs;
            foreach ((T v1, T v2, bool orientedEdge) in edges)
            {
                foreach (var subgraph in subgraphs) subgraph.TryConnect(v1, v2, orientedEdge);
            }
            return subgraphs;
        }
        /// <summary>
        /// Creates subgraphs built on strong connected components of specified weighted graph.
        /// </summary>
        /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
        /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
        /// <param name="graph">Weighted graph to build subgraphs built on strong connected components of.</param>
        /// <returns>List of subgraphs built on strong connected components of specified weighted graph.</returns>
        public static List<WeightedGraph<TVertex, TEdgeWeight>>
        CreateSubgraphsFromStrongConnectedComponents<TVertex, TEdgeWeight>(
        WeightedGraph<TVertex, TEdgeWeight> graph) where TVertex : notnull
        {
            var components = StrongConnectedComponents(graph);
            List<WeightedGraph<TVertex, TEdgeWeight>> subgraphs = new();
            foreach (HashSet<TVertex> component in components)
                subgraphs.Add(WeightedGraph<TVertex, TEdgeWeight>.Create(component));
            var edges = graph.Edges();
            if (edges is null) return subgraphs;
            foreach ((TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight weight) in edges)
            {
                foreach (var subgraph in subgraphs) subgraph.TryConnect(v1, v2, orientedEdge, weight);
            }
            return subgraphs;
        }
    }
}
