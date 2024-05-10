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
        static void GraphExceptionCheck2<T>(Graph<T> graph, T start, T destination) where T : notnull
        {
            GraphExceptionsCheck(graph, start);
            if (!graph.HasVertex(destination)) throw new InvalidOperationException($"Specified vertex '{destination}' was not in the graph.");
        }
        static void WeightedGraphExceptionsCheck<TVertex, TEdgeWeight>(WeightedGraph<TVertex, TEdgeWeight> graph, TVertex start) 
            where TVertex : notnull
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            if (graph.VertexCount == 0) throw new InvalidOperationException($"Specified graph '{graph}' had no vertices.");
            if (!graph.HasVertex(start)) throw new InvalidOperationException($"Specified vertex '{start}' was not in the graph.");
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
        public static Dictionary<T, int> ShortestPathLengths<T>(Graph<T> graph, T start) where T : notnull
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
        public static (Dictionary<T, int> PathLengths, Dictionary<T, T> Paths) ShortestPathsAndPathLengths<T>(Graph<T> graph, T start) where T : notnull
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
        public static Dictionary<T, T> ShortestPaths<T>(Graph<T> graph, T start) where T : notnull
            => ShortestPathsAndPathLengths(graph, start).Paths;
        public static int ShortestPathLength<T>(Graph<T> graph, T start, T destination) where T : notnull
        {
            GraphExceptionCheck2(graph, start, destination);
            if (EqualityComparer<T>.Default.Equals(start, destination)) return 0;
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
                if (EqualityComparer<T>.Default.Equals(closestVertex, destination)) return pathLengths[destination];
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return -1;
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
        public static (int PathLength, List<T>? Path) ShortestPathAndPathLength<T>(Graph<T> graph, T start, T destination) where T : notnull
        {
            GraphExceptionCheck2(graph, start, destination);
            if (EqualityComparer<T>.Default.Equals(start, destination)) return (0, null);
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
                if (EqualityComparer<T>.Default.Equals(closestVertex, destination))
                    return (lowestDistance, PrivateBuildPath(prevsInPath, destination));
                front.Remove(closestVertex);
                visited.Add(closestVertex);
                OpenVertex(closestVertex);
            }
            return (-1, null);
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
        public static List<T>? ShortestPath<T>(Graph<T> graph, T start, T destination) where T : notnull =>
            ShortestPathAndPathLength(graph, start, destination).Path;
        public static Dictionary<TVertex, TEdgeWeight> ShortestPathLengths<TVertex, TEdgeWeight>(
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
        public static (Dictionary<TVertex, TEdgeWeight> PathLengths, Dictionary<TVertex, TVertex> Paths)
            ShortestPathsAndPathLengths<TVertex, TEdgeWeight>(
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
        public static Dictionary<TVertex, TVertex> ShortestPaths<TVertex, TEdgeWeight>
            (WeightedGraph<TVertex, TEdgeWeight> graph,
            TVertex start,
            IComparer<TEdgeWeight> comparer,
            Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
            => ShortestPathsAndPathLengths(graph, start, comparer, edgeWeightAddFunction).Paths;
        public static List<T> BuildPath<T>(Dictionary<T, T> paths, T destination) where T : notnull
        {
            if (paths is null) throw new ArgumentNullException($"Specified paths dictionary '{paths}' was null.");
            if (!paths.ContainsKey(destination)) throw new InvalidOperationException($"Specified vertex '{destination}' was unreachable.");
            return PrivateBuildPath<T>(paths, destination);
        }
    }
}
