﻿namespace CSharpGraphsLibrary
{
    public static partial class Paths
    {
        static void GraphExceptionCheck<T>(Graph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException(
                $"Specified vertex '{start}' was not in the graph.");
        }
        static void GraphExceptionCheck2<T>(Graph<T> graph, T start, T destination) where T : notnull
        {
            GraphExceptionCheck(graph, start);
            if (!graph.HasVertex(destination)) throw new InvalidOperationException(
                $"Specified vertex '{destination}' was not in the graph.");
        }
        static void WeightedGraphExceptionCheck<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph, TVertex start,
            Comparison<TEdgeWeight> comparison,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction)
            where TVertex : notnull
        {
            if (graph is null) throw new ArgumentNullException(
                $"Specified graph '{graph}' was null.");
            if (comparison is null) throw new ArgumentNullException(
                $"Specified edge weight comparison function '{comparison}' was null.");
            if (edgeWeightAddFunction is null) throw new ArgumentNullException(
                $"Specified edge weight add function '{edgeWeightAddFunction}' was null.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException(
                $"Specified vertex '{start}' was not in the graph.");
        }
        static void WeightedGraphExceptionCheck2<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph,
            TVertex start, TVertex destination,
            Comparison<TEdgeWeight> comparison,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction,
            Func<TVertex, TVertex, TEdgeWeight> heuristic)
            where TVertex : notnull
        {
            WeightedGraphExceptionCheck(graph, start, comparison, edgeWeightAddFunction);
            if (!graph.HasVertex(destination)) throw new InvalidOperationException(
                $"Specified vertex '{destination}' was not in the graph.");
            if (heuristic is null) throw new ArgumentNullException(
                $"Specified heuristic function '{heuristic}' was null.");
        }
        static List<T> PrivateBuildPath<T>(Dictionary<T, T> paths, T destination) where T : notnull
        {
            List<T> result = new();
            T current = destination;
            while (paths.TryGetValue(current, out T? prev))
            {
                result.Add(current);
                current = prev;
            }
            result.Add(current);
            result.Reverse();
            return result;
        }
        public static List<T> BuildPath<T>(Dictionary<T, T> paths, T destination) where T : notnull
        {
            if (paths is null) throw new ArgumentNullException($"Specified paths dictionary '{paths}' was null.");
            if (!paths.ContainsKey(destination)) throw new InvalidOperationException($"Specified vertex '{destination}' was unreachable.");
            return PrivateBuildPath<T>(paths, destination);
        }
    }
}
