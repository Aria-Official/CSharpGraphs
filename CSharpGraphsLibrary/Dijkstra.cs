namespace CSharpGraphsLibrary
{
    public static class Dijkstra
    {
        static void GraphExceptionsCheck<T>(Graph<T> graph, T start) where T : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException($"Specified vertex '{start}' was not in the graph.");
        }
        static void WeightedGraphExceptionsCheck<TVertex, TEdgeWeight>(WeightedGraph<TVertex, TEdgeWeight> graph, TVertex start) 
            where TVertex : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException($"Specified vertex '{start}' was not in the graph.");
        }
        public static Dictionary<T, int> ShortestPaths<T>(Graph<T> graph, T start) where T : notnull
        {
            GraphExceptionsCheck(graph, start);
            Dictionary<T, int> pathLengths = new() { { start, 0 } };
            HashSet<T> visited = new() { start }, front = new();
            OpenVertex(start);
            int lowestDistance = int.MaxValue;  // Initialization is required to compile.
            T closestVertex = default!;         // Initialization is required to compile.
            while (front.Count > 0)
            {
                foreach (T vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                {
                    lowestDistance = pathLengths[vertex];
                    closestVertex = vertex;
                    break;
                }
                foreach (T vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                {
                    int distance = pathLengths[vertex];
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        closestVertex = vertex;
                    }
                }
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return pathLengths;
            void OpenVertex(T vertex)
            {
                foreach (T neighbour in graph.NeighboursOf(vertex)!)
                {
                    bool anyPath = pathLengths.TryGetValue(neighbour, out int path);
                    int newPath = pathLengths[vertex] + 1;
                    if (anyPath)
                    {
                        if (newPath < path) pathLengths[neighbour] = newPath;
                    }
                    else pathLengths.Add(neighbour, newPath);
                    if (!visited.Contains(neighbour) && !front.Contains(neighbour)) front.Add(neighbour);
                }
            }
        }
        public static (Dictionary<T, int> PathLengths, Dictionary<T, T> PrevsInPath) ShortestPathsAndPrevs<T>(Graph<T> graph, T start) where T : notnull
        {
            GraphExceptionsCheck(graph, start);
            Dictionary<T, T> prevsInPath = new();
            Dictionary<T, int> pathLengths = new() { { start, 0 } };
            HashSet<T> visited = new() { start }, front = new();
            OpenVertex(start);
            int lowestDistance = int.MaxValue;  // Initialization is required to compile.
            T closestVertex = default!;         // Initialization is required to compile.
            while (front.Count > 0)
            {
                foreach (T vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                {
                    lowestDistance = pathLengths[vertex];
                    closestVertex = vertex;
                    break;
                }
                foreach (T vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                {
                    int distance = pathLengths[vertex];
                    if (distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        closestVertex = vertex;
                    }
                }
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return (pathLengths, prevsInPath);
            void OpenVertex(T vertex)
            {
                foreach (T neighbour in graph.NeighboursOf(vertex)!)
                {
                    bool anyPath = pathLengths.TryGetValue(neighbour, out int path);
                    int newPath = pathLengths[vertex] + 1;
                    if (anyPath)
                    {
                        if (newPath < path)
                        {
                            pathLengths[neighbour] = newPath;
                            prevsInPath[neighbour] = vertex;
                        }
                    }
                    else
                    {
                        pathLengths.Add(neighbour, newPath);
                        prevsInPath.Add(neighbour, vertex);
                    }
                    if (!visited.Contains(neighbour) && !front.Contains(neighbour)) front.Add(neighbour);
                }
            }
        }
        public static Dictionary<T, T> Prevs<T>(Graph<T> graph, T start) where T : notnull => ShortestPathsAndPrevs(graph, start).PrevsInPath;
        public static (Dictionary<TVertex, TEdgeWeight> PathLengths, Dictionary<TVertex, TVertex> PrevsInPath)
            ShortestPathsAndPrevs<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph,
            TVertex start,
            IComparer<TEdgeWeight> comparer,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
        {
            WeightedGraphExceptionsCheck(graph, start);
            Dictionary<TVertex, TVertex> prevsInPath = new();
            Dictionary<TVertex, TEdgeWeight> pathLengths = new() { { start, default! } };
            HashSet<TVertex> visited = new() { start }, front = new();
            OpenVertex(start);
            TEdgeWeight lowestDistance = default!;  // Initialization is required to compile.
            TVertex closestVertex = default!;       // Initialization is required to compile.
            while (front.Count > 0)
            {
                foreach (TVertex vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                {
                    lowestDistance = pathLengths[vertex];
                    closestVertex = vertex;
                    break;
                }
                foreach (TVertex vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                {
                    TEdgeWeight distance = pathLengths[vertex];
                    if (comparer.Compare(distance, lowestDistance) < 0)
                    {
                        lowestDistance = distance;
                        closestVertex = vertex;
                    }
                }
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return (pathLengths, prevsInPath);
            void OpenVertex(TVertex vertex)
            {
                foreach ((TVertex neighbour, TEdgeWeight weight) in graph.NeighboursWithWeightOf(vertex)!)
                {
                    bool anyPath = pathLengths.TryGetValue(neighbour, out TEdgeWeight? path);
                    TEdgeWeight newpath = edgeWeightAddFunction(pathLengths[vertex], weight);
                    if (anyPath)
                    {
                        if (comparer.Compare(newpath, path) < 0)
                        {
                            pathLengths[neighbour] = newpath;
                            prevsInPath[neighbour] = vertex;
                        }
                    }
                    else
                    {
                        pathLengths.Add(neighbour, newpath);
                        prevsInPath.Add(neighbour, vertex);
                    }
                    if (!visited.Contains(neighbour) && !front.Contains(neighbour)) front.Add(neighbour);
                }
            }
        }
        public static Dictionary<TVertex, TEdgeWeight> ShortestPaths<TVertex, TEdgeWeight>(
            WeightedGraph<TVertex, TEdgeWeight> graph,
            TVertex start,
            IComparer<TEdgeWeight> comparer,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
        {
            WeightedGraphExceptionsCheck(graph, start);
            Dictionary<TVertex, TEdgeWeight> pathLengths = new() { { start, default! } };
            HashSet<TVertex> visited = new() { start }, front = new();
            OpenVertex(start);
            TEdgeWeight lowestDistance = default!;  // Initialization is required to compile.
            TVertex closestVertex = default!;       // Initialization is required to compile.
            while (front.Count > 0)
            {
                foreach (TVertex vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                {
                    lowestDistance = pathLengths[vertex];
                    closestVertex = vertex;
                    break;
                }
                foreach (TVertex vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                {
                    TEdgeWeight distance = pathLengths[vertex];
                    if (comparer.Compare(distance, lowestDistance) < 0)
                    {
                        lowestDistance = distance;
                        closestVertex = vertex;
                    }
                }
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return pathLengths;
            void OpenVertex(TVertex vertex)
            {
                foreach ((TVertex neighbour, TEdgeWeight weight) in graph.NeighboursWithWeightOf(vertex)!)
                {
                    bool anyPath = pathLengths.TryGetValue(neighbour, out TEdgeWeight? path);
                    TEdgeWeight newpath = edgeWeightAddFunction(pathLengths[vertex], weight);
                    if (anyPath)
                    {
                        if (comparer.Compare(newpath, path) < 0) pathLengths[neighbour] = newpath;
                    }
                    else pathLengths.Add(neighbour, newpath);
                    if (!visited.Contains(neighbour) && !front.Contains(neighbour)) front.Add(neighbour);
                }
            }
        }
        public static Dictionary<TVertex, TVertex> Prevs<TVertex, TEdgeWeight>
            (WeightedGraph<TVertex, TEdgeWeight> graph,
            TVertex start,
            IComparer<TEdgeWeight> comparer,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
            => ShortestPathsAndPrevs(graph, start, comparer, edgeWeightAddFunction).PrevsInPath;
    }
}
