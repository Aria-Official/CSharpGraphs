namespace CSharpGraphsLibrary
{
    public class Graph<T> where T : notnull
    {
        readonly Dictionary<T, HashSet<T>> mapping;
        public int VertexCount => mapping.Keys.Count;
        public int EdgeCount { get; private set; }
        Graph()
        {
            mapping = new();
            EdgeCount = 0;
        }
        Graph(IEnumerable<T> vertices) : this()
        {
            foreach (T v in vertices) mapping.TryAdd(v, new());
        }
        public static Graph<T> Create() => new();
        public static Graph<T> Create(IEnumerable<T> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                $"Create() failed. Specified enumerable collection of vertices '{vertices}' was null.");
            return new Graph<T>(vertices);
        }
        public static Graph<T> Create(params T[] vertices) => new(vertices);
        public void AddVertex(T v)
        {
            if (!mapping.TryAdd(v, new())) throw new InvalidOperationException(
                $"AddVertex() failed. Specified vertex '{v}' was already in the graph.");
        }
        public void RemoveVertex(T v)
        {
            if (!mapping.ContainsKey(v)) throw new InvalidOperationException(
                $"RemoveVertex() was impossible. Specified vertex '{v}' was not in the graph.");
            foreach (T neighbourForward in mapping[v]) Disconnect(v, neighbourForward);
            foreach (T neighbourBackward in mapping.Keys) Disconnect(neighbourBackward, v);
            mapping.Remove(v);
        }
        public bool HasVertex(T v) => mapping.ContainsKey(v);
        public HashSet<T> NeighboursOf(T v)
        {
            bool found = mapping.TryGetValue(v, out var neighbours);
            if (found) return neighbours!;
            throw new InvalidOperationException(
                $"NeighboursOf() failed. Specified vertex '{v}' was not in the graph.");
        }
        public void Connect(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v2}' was not in the graph.");
            if (mapping[v1].Contains(v2))
            {
                if (mapping[v2].Contains(v1))
                {
                    if (orientedEdge) mapping[v2].Remove(v1);
                }
                else if (!orientedEdge) mapping[v2].Add(v1);
            }
            else
            {
                if (mapping[v2].Contains(v1))
                {
                    if (!orientedEdge) return;
                    mapping[v2].Remove(v1);
                    mapping[v1].Add(v2);
                }
                else
                {
                    mapping[v1].Add(v2);
                    if (!orientedEdge) mapping[v2].Add(v1);
                    ++EdgeCount;
                }
            }
        }
        public void Disconnect(T v1, T v2)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v2}' was not in the graph.");
            if (mapping[v1].Remove(v2) | mapping[v2].Remove(v1)) --EdgeCount;
        }
        public bool HasEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1) || !mapping.ContainsKey(v2)) return false;
            if (orientedEdge) return mapping[v1].Contains(v2);
            return mapping[v1].Contains(v2) && mapping[v2].Contains(v1);
        }
        public HashSet<T> Vertices()
        {
            HashSet<T> vertices = new();
            foreach (T vertex in mapping.Keys) vertices.Add(vertex);
            return vertices;
        }
        public HashSet<(T, T, bool)> Edges()
        {
            HashSet<(T, T, bool)> edges = new();
            foreach (T v in mapping.Keys)
            {
                foreach (T neighbour in mapping[v])
                {
                    if (edges.Contains((neighbour, v, true)))
                    {
                        edges.Remove((neighbour, v, true));
                        edges.Add((neighbour, v, false));
                    }
                    else edges.Add((v, neighbour, true));
                }
            }
            return edges;
        }
    }
}
