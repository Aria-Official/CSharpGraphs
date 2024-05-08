namespace CSharpGraphsLibrary
{
    public class WeightedGraph<TVertex, TEdgeWeight> where TVertex : notnull
    {
        readonly Dictionary<TVertex, Dictionary<TVertex, TEdgeWeight>> mapping;
        public int VertexCount => mapping.Keys.Count;
        public int EdgeCount { get; private set; }
        WeightedGraph()
        {
            mapping = new();
            EdgeCount = 0;
        }
        WeightedGraph(IEnumerable<TVertex> vertices) : this()
        {
            foreach (TVertex v in vertices) mapping.TryAdd(v, new());
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create() => new();
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IEnumerable<TVertex> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                $"Specified enumerable collection of vertices '{vertices}' was null.");
            return new WeightedGraph<TVertex, TEdgeWeight>(vertices);
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create(params TVertex[] vertices) => new(vertices);
        public void AddVertex(TVertex v)
        {
            if (!mapping.TryAdd(v, new())) throw new InvalidOperationException(
                $"Specified vertex '{v}' was already in the graph.");
        }
        public bool TryAddVertex(TVertex v) => mapping.TryAdd(v, new());
        public void RemoveVertex(TVertex v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) throw new InvalidOperationException(
                $"Specified vertex '{v}' was not in the graph.");
            foreach ((TVertex pointed, TEdgeWeight _) in neighbours) Disconnect(v, pointed);
            foreach (TVertex pointing in mapping.Keys) Disconnect(pointing, v);
            mapping.Remove(v);
        }
        public bool TryRemoveVertex(TVertex v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) return false;
            foreach ((TVertex pointed, TEdgeWeight _) in neighbours) Disconnect(v, pointed);
            foreach (TVertex pointing in mapping.Keys) Disconnect(pointing, v);
            return mapping.Remove(v); // This line is designed to always return true.
        }
        public bool HasVertex(TVertex v) => mapping.ContainsKey(v);
        public IEnumerable<TVertex>? NeighboursOf(TVertex v)
        {
            bool found = mapping.TryGetValue(v, out var neighbours);
            if (found) return neighbours!.Keys;
            return null;
        }
        public IReadOnlyDictionary<TVertex, TEdgeWeight>? NeighboursWithWeightOf(TVertex v)
        {
            bool found = mapping.TryGetValue(v, out var neighbours);
            if (found) return neighbours!;
            return null;
        }
        void Connect(TVertex v1, TVertex v2, Dictionary<TVertex, TEdgeWeight> dict1,
            Dictionary<TVertex, TEdgeWeight> dict2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (dict1.ContainsKey(v2))
            {
                if (dict2.ContainsKey(v1))
                {
                    dict1[v2] = edgeWeight;
                    if (orientedEdge) dict2.Remove(v1);
                    else dict2[v1] = edgeWeight;
                }
                else
                {
                    dict1[v2] = edgeWeight;
                    if (!orientedEdge) dict2.Add(v1, edgeWeight);
                }
            }
            else
            {
                if (dict2.ContainsKey(v1))
                {
                    dict1.Add(v2, edgeWeight);
                    if (orientedEdge) dict2.Remove(v1);
                    else dict2[v1] = edgeWeight;
                }
                else
                {
                    dict1.Add(v2, edgeWeight);
                    if (!orientedEdge) dict2.Add(v1, edgeWeight);
                    ++EdgeCount;
                }
            }
        }
        public void Connect(TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) throw new InvalidOperationException(
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var dict2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
            Connect(v1, v2, dict1, dict2, orientedEdge, edgeWeight);
        }
        public bool TryConnect(TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) return false;
            if (!mapping.TryGetValue(v2, out var dict2)) return false;
            Connect(v1, v2, dict1, dict2, orientedEdge, edgeWeight);
            return true;
        }
        public void Disconnect(TVertex v1, TVertex v2)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) throw new InvalidOperationException(
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var dict2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
            if (dict1.Remove(v2) | dict2.Remove(v1)) --EdgeCount;
        }
        public bool TryDisconnect(TVertex v1, TVertex v2)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) return false;
            if (!mapping.TryGetValue(v2, out var dict2)) return false;
            if (dict1.Remove(v2) | dict2.Remove(v1)) { --EdgeCount; return true; }
            return false;
        }
        public bool HasEdge(TVertex v1, TVertex v2, bool orientedEdge)
        {
            if (!mapping.TryGetValue(v1, out var dict1) || !mapping.TryGetValue(v2, out var dict2)) return false;
            if (orientedEdge) return dict1.ContainsKey(v2);
            return dict1.ContainsKey(v2) & dict2.ContainsKey(v1);
        }

        public bool HasEdge(TVertex v1, TVertex v2, bool orientedEdge, out TEdgeWeight? edgeWeight)
        {
            if (!mapping.TryGetValue(v1, out var dict1) || !mapping.TryGetValue(v2, out var dict2))
            {
                edgeWeight = default;
                return false;
            }
            if (orientedEdge)
            {
                if (dict1.ContainsKey(v2)) { edgeWeight = dict1[v2]; return true; }
                else { edgeWeight = default; return false; }
            }
            else
            {
                if (dict1.ContainsKey(v2) & dict2.ContainsKey(v1)) { edgeWeight = dict1[v2]; return true; }
                else { edgeWeight = default; return false; }
            }
        }
        public HashSet<TVertex>? Vertices()
        {
            if (VertexCount == 0) return null;
            HashSet<TVertex> vertices = new();
            vertices.UnionWith(mapping.Keys);
            return vertices;
        }
        public HashSet<(TVertex, TVertex, bool, TEdgeWeight)>? Edges()
        {
            if (EdgeCount == 0) return null;
            HashSet<(TVertex, TVertex, bool, TEdgeWeight)> edges = new();
            foreach (TVertex v in mapping.Keys)
            {
                foreach ((TVertex neighbour, TEdgeWeight edgeWeight) in mapping[v])
                {
                    if (edges.Contains((neighbour, v, true, edgeWeight)))
                    {
                        edges.Remove((neighbour, v, true, edgeWeight));
                        edges.Add((neighbour, v, false, edgeWeight));
                    }
                    else edges.Add((v, neighbour, true, edgeWeight));
                }
            }
            return edges;
        }
    }
}
