﻿namespace CSharpGraphsLibrary
{
    public static class Traversals
    {
        public static IEnumerable<T> DepthFirstTraversal<T>(ITraversableGraph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
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
        public static IEnumerable<T> BreadthFirstTraversal<T>(ITraversableGraph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
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