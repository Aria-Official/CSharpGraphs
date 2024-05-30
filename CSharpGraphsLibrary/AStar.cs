namespace CSharpGraphsLibrary
{
    public static partial class Paths
    {
        /// <summary>
        /// Provides container for weighted graph shortest path algorithms implemented on idea of A* algorithm.
        /// </summary>
        public static class AStar
        {
            /// <summary>
            /// Finds length of shortest path from specified start vertex to specified destination vertex on specified weighted graph.<br/>
            /// A* is faster than Dijkstra's algorithm, but doesn't guarantee to find shortest path.
            /// </summary>
            /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
            /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
            /// <param name="graph">Graph ot look for path length on.</param>
            /// <param name="start">Vertex to be start of the path.</param>
            /// <param name="destination">Vertex to be destination of the path.</param>
            /// <param name="comparison">Method to compare objects of graph edge weight.</param>
            /// <param name="edgeWeightAddFunction">Method to add together objects of graph edge weight.</param>
            /// <param name="heuristic">Method to calculate heuristic edge weight value based on two given vertices.</param>
            /// <returns>Length of shortest path between start and destination vertices.</returns>
            public static TEdgeWeight? ShortestPathLength<TVertex, TEdgeWeight>(
                WeightedGraph<TVertex, TEdgeWeight> graph,
                TVertex start, TVertex destination,
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction,
                Func<TVertex, TVertex, TEdgeWeight> heuristic) where TVertex : notnull
            {
                WeightedGraphExceptionCheck2(
                    graph, start, destination, comparison, edgeWeightAddFunction, heuristic);
                if (EqualityComparer<TVertex>.Default.Equals(start, destination)) return default;
                Dictionary<TVertex, TEdgeWeight> pathLengths = new() { { start, default! } };
                HashSet<TVertex> visited = new() { start }, front = new();
                OpenVertex(start);
                TEdgeWeight lowestHeuristicDistance = default!;  // Initialization is required to compile.
                TVertex prioritizedVertex = default!;            // Initialization is required to compile.
                while (front.Count > 0)
                {
                    foreach (TVertex vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                    {
                        lowestHeuristicDistance = edgeWeightAddFunction(pathLengths[vertex], heuristic(vertex, destination));
                        prioritizedVertex = vertex;
                        break;
                    }
                    foreach (TVertex vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                    {
                        TEdgeWeight distance = edgeWeightAddFunction(pathLengths[vertex], heuristic(vertex, destination));
                        if (comparison(distance, lowestHeuristicDistance) < 0)
                        {
                            lowestHeuristicDistance = distance;
                            prioritizedVertex = vertex;
                        }
                    }
                    if (EqualityComparer<TVertex>.Default.Equals(prioritizedVertex, destination))
                        return pathLengths[destination];
                    front.Remove(prioritizedVertex);
                    visited.Add(prioritizedVertex);
                    OpenVertex(prioritizedVertex);
                }
                return default; // If destination is unreachable from start return default.
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
            /// <summary>
            /// Finds:<br/>
            /// 1. length of shortest path,<br/>
            /// 2. shortest path as list of vertices<br/>
            /// from specified start vertex to specified destination vertex on specified weighted graph.<br/>
            /// A* is faster than Dijkstra's algorithm, but doesn't guarantee to find shortest path.
            /// </summary>
            /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
            /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
            /// <param name="graph">Graph ot look for path length and path on.</param>
            /// <param name="start">Vertex to be start of the path.</param>
            /// <param name="destination">Vertex to be destination of the path.</param>
            /// <param name="comparison">Method to compare objects of graph edge weight.</param>
            /// <param name="edgeWeightAddFunction">Method to add together objects of graph edge weight.</param>
            /// <param name="heuristic">Method to calculate heuristic edge weight value based on two given vertices.</param>
            /// <returns>Pair of:<br/>
            /// 1. length of shortest path;<br/>
            /// 2. shortest path as list of vertices.
            /// </returns>
            public static (TEdgeWeight? PathLength, List<TVertex>? Path)
                ShortestPathAndPathLength<TVertex, TEdgeWeight>(
                WeightedGraph<TVertex, TEdgeWeight> graph,
                TVertex start, TVertex destination,
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction,
                Func<TVertex, TVertex, TEdgeWeight> heuristic) where TVertex : notnull
            {
                WeightedGraphExceptionCheck2(
                    graph, start, destination, comparison, edgeWeightAddFunction, heuristic);
                if (EqualityComparer<TVertex>.Default.Equals(start, destination)) return (default, null);
                Dictionary<TVertex, TVertex> prevsInPath = new();
                Dictionary<TVertex, TEdgeWeight> pathLengths = new() { { start, default! } };
                HashSet<TVertex> visited = new() { start }, front = new();
                OpenVertex(start);
                TEdgeWeight lowestHeuristicDistance = default!;  // Initialization is required to compile.
                TVertex prioritizedVertex = default!;            // Initialization is required to compile.
                while (front.Count > 0)
                {
                    foreach (TVertex vertex in front) // Designed to iterate only once, marking first vertex in front as closest.
                    {
                        lowestHeuristicDistance = edgeWeightAddFunction(pathLengths[vertex], heuristic(vertex, destination));
                        prioritizedVertex = vertex;
                        break;
                    }
                    foreach (TVertex vertex in front) // Designed to get the reference to actual closest vertex to pop it from front.
                    {
                        TEdgeWeight distance = edgeWeightAddFunction(pathLengths[vertex], heuristic(vertex, destination));
                        if (comparison(distance, lowestHeuristicDistance) < 0)
                        {
                            lowestHeuristicDistance = distance;
                            prioritizedVertex = vertex;
                        }
                    }
                    if (EqualityComparer<TVertex>.Default.Equals(prioritizedVertex, destination))
                        return (pathLengths[destination], PrivateBuildPath(prevsInPath, destination));
                    front.Remove(prioritizedVertex);
                    visited.Add(prioritizedVertex);
                    OpenVertex(prioritizedVertex);
                }
                return (default, null); // If destination is unreachable from start return default as path length and null as path.
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
            /// <summary>
            /// Finds shortest path as list of vertices from specified start vertex to specified destination vertex on specified weighted graph.<br/>
            /// A* is faster than Dijkstra's algorithm, but doesn't guarantee to find shortest path.
            /// </summary>
            /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
            /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
            /// <param name="graph">Graph ot look for path on.</param>
            /// <param name="start">Vertex to be start of the path.</param>
            /// <param name="destination">Vertex to be destination of the path.</param>
            /// <param name="comparison">Method to compare objects of graph edge weight.</param>
            /// <param name="edgeWeightAddFunction">Method to add together objects of graph edge weight.</param>
            /// <param name="heuristic">Method to calculate heuristic edge weight value based on two given vertices.</param>
            /// <returns>Shortest path between start and destination vertices as list of vertices.</returns>
            public static List<TVertex>? ShortestPath<TVertex, TEdgeWeight>(
                WeightedGraph<TVertex, TEdgeWeight> graph,
                TVertex start, TVertex destination,
                Comparison<TEdgeWeight> comparison,
                Func<TEdgeWeight, TEdgeWeight, TEdgeWeight> edgeWeightAddFunction,
                Func<TVertex, TVertex, TEdgeWeight> heuristic) where TVertex : notnull
                => ShortestPathAndPathLength(
                    graph, start, destination, comparison, edgeWeightAddFunction, heuristic).Path;
        }
    }
}
