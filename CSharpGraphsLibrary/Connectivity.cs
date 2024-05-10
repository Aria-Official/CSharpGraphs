namespace CSharpGraphsLibrary
{
    public static class Connectivity
    {
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
        public static bool IsStrongConnected<T>(ITraversableGraph<T> graph) where T : notnull
            => StrongConnectedComponents(graph).Count == 1;
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
