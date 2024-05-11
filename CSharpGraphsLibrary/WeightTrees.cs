namespace CSharpGraphsLibrary
{
    public static class WeightTrees
    {
        public static List<(TVertex, TVertex, TEdgeWeight)> WeightTree<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph, TVertex start,
            Comparison<TEdgeWeight> comparison) where TVertex : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph} was null.'");
            if (!graph.HasVertex(start)) throw new InvalidOperationException(
                $"Specified vertex '{start}' was not in the graph.");
            if (comparison is null) throw new ArgumentNullException(
                $"Specified edge weight comparison function '{comparison}' was null.");
            PriorityQueue<(TVertex, TVertex), TEdgeWeight> queue = new(Comparer<TEdgeWeight>.Create(comparison));
            int joinedVertices = 1;
            List<(TVertex, TVertex, TEdgeWeight)> weightTree = new();
            foreach ((TVertex neighbour, TEdgeWeight weight) in graph.NeighboursWithWeightOf(start)!)
                queue.Enqueue((start, neighbour), weight);
            HashSet<TVertex> visited = new() { start };
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
