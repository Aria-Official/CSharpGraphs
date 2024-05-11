namespace CSharpGraphsLibrary
{
    public static partial class Paths
    {
        public static class Dijkstra
        {
            public static Dictionary<T, int> ShortestPathLengths<T>(Graph<T> graph, T start) where T : notnull
            {
                GraphExceptionCheck(graph, start);
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
                GraphExceptionCheck(graph, start);
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
                return -1; // If destination is unreachable from start return -1.
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
                return (-1, null); // If destination is unreachable from start return -1 as path length and null as path.
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
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
            {
                WeightedGraphExceptionCheck(graph, start, comparison, edgeWeightAddFunction);
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
                        if (comparison(distance, lowestDistance) < 0)
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
                            if (comparison(newpath, path!) < 0) pathLengths[neighbour] = newpath;
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
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
            {
                WeightedGraphExceptionCheck(graph, start, comparison, edgeWeightAddFunction);
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
                        if (comparison(distance, lowestDistance) < 0)
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
                            if (comparison(newpath, path!) < 0)
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
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction) where TVertex : notnull
                => ShortestPathsAndPathLengths(graph, start, comparison, edgeWeightAddFunction).Paths;
        }
    }
}
