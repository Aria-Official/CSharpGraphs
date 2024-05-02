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
                $"Create() failed. Specified enumerable collection of vertices '{vertices}' was null.");
            return new WeightedGraph<TVertex, TEdgeWeight>(vertices);
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create(params TVertex[] vertices) => new(vertices);
        public void AddVertex(TVertex v)
        {
            if (!mapping.TryAdd(v, new())) throw new InvalidOperationException(
                $"AddVertex() failed. Specified vertex '{v}' was already in the graph.");
        }
        public void RemoveVertex(TVertex v)
        {
            if (!mapping.ContainsKey(v)) throw new InvalidOperationException(
                $"RemoveVertex() was impossible. Specified vertex '{v}' was not in the graph.");
            foreach ((TVertex neighbourForward, TEdgeWeight _) in mapping[v]) Disconnect(v, neighbourForward);
            foreach (TVertex neighbourBackward in mapping.Keys) Disconnect(neighbourBackward, v);
            mapping.Remove(v);
        }
        public bool HasVertex(TVertex v) => mapping.ContainsKey(v);
        public Dictionary<TVertex, TEdgeWeight> NeighboursOf(TVertex v)
        {
            bool found = mapping.TryGetValue(v, out var neighbours);
            if (found) return neighbours!;
            throw new InvalidOperationException(
                $"NeighboursOf() failed. Specified vertex '{v}' was not in the graph.");
        }
        public void Connect(TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v2}' was not in the graph.");
            if (mapping[v1].ContainsKey(v2))
            {
                if (mapping[v2].ContainsKey(v1))
                {
                    mapping[v1][v2] = edgeWeight;
                    if (orientedEdge) mapping[v2].Remove(v1);
                    else mapping[v2][v1] = edgeWeight;
                }
                else
                {
                    mapping[v1][v2] = edgeWeight;
                    if (!orientedEdge) mapping[v2].Add(v1, edgeWeight);
                }
            }
            else
            {
                if (mapping[v2].ContainsKey(v1))
                {
                    mapping[v1].Add(v2, edgeWeight);
                    if (orientedEdge) mapping[v2].Remove(v1);
                    else mapping[v2][v1] = edgeWeight;
                }
                else
                {
                    mapping[v1].Add(v2, edgeWeight);
                    if (!orientedEdge) mapping[v2].Add(v1, edgeWeight);
                }
            }
        }
        public void Disconnect(TVertex v1, TVertex v2)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v2}' was not in the graph.");
            if (mapping[v1].Remove(v2) | mapping[v2].Remove(v1)) --EdgeCount;
        }
        public bool HasEdge(TVertex v1, TVertex v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1) || !mapping.ContainsKey(v2)) return false;
            if (orientedEdge) return mapping[v1].ContainsKey(v2);
            return mapping[v1].ContainsKey(v2) & mapping[v2].ContainsKey(v1);
        }
        public HashSet<TVertex> Vertices()
        {
            HashSet<TVertex> vertices = new();
            foreach (TVertex vertex in mapping.Keys) vertices.Add(vertex);
            return vertices;
        }
        public HashSet<(TVertex, TVertex, bool, TEdgeWeight)> Edges()
        {
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
