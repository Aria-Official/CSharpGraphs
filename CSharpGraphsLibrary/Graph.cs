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
        public bool TryAddVertex(T v) => mapping.TryAdd(v, new());
        public void RemoveVertex(T v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) throw new InvalidOperationException(
                $"RemoveVertex() was impossible. Specified vertex '{v}' was not in the graph.");
            foreach (T pointed in neighbours!) Disconnect(v, pointed);
            foreach (T pointing in mapping.Keys) Disconnect(pointing, v);
            mapping.Remove(v);
        }
        public bool TryRemoveVertex(T v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) return false;
            foreach (T pointed in neighbours!) Disconnect(v, pointed);
            foreach (T pointing in mapping.Keys) Disconnect(pointing, v);
            return mapping.Remove(v); // This line is designed to always return true.
        }
        public bool HasVertex(T v) => mapping.ContainsKey(v);
        public IReadOnlySet<T>? NeighboursOf(T v)
        {
            if (mapping.TryGetValue(v, out var neighbours)) return neighbours!;
            return null;
        }
        void Connect(T v1, T v2, HashSet<T> set1, HashSet<T> set2, bool orientedEdge)
        {
            if (set1.Contains(v2))
            {
                if (set2.Contains(v1))
                {
                    if (orientedEdge) set2.Remove(v1);
                }
                else if (!orientedEdge) set2.Add(v1);
            }
            else
            {
                if (set2.Contains(v1))
                {
                    if (!orientedEdge) return;
                    set2.Remove(v1);
                    set1.Add(v2);
                }
                else
                {
                    set1.Add(v2);
                    if (!orientedEdge) set2.Add(v1);
                    ++EdgeCount;
                }
            }
        }
        public void Connect(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.TryGetValue(v1, out var set1)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var set2)) throw new InvalidOperationException(
                $"Connect() failed. Specified vertex '{v2}' was not in the graph.");
            Connect(v1, v2, set1, set2, orientedEdge);
        }
        public bool TryConnect(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.TryGetValue(v1, out var set1)) return false;
            if (!mapping.TryGetValue(v2, out var set2)) return false;
            Connect(v1, v2, set1, set2, orientedEdge);
            return true;
        }
        public void Disconnect(T v1, T v2)
        {
            if (!mapping.TryGetValue(v1, out var set1)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var set2)) throw new InvalidOperationException(
                $"Disconnect() was impossible. Specified vertex '{v2}' was not in the graph.");
            if (set1.Remove(v2) | set2.Remove(v1)) --EdgeCount;
        }
        public bool TryDisconnect(T v1, T v2)
        {
            if (!mapping.TryGetValue(v1, out var set1)) return false;
            if (!mapping.TryGetValue(v2, out var set2)) return false;
            if (set1.Remove(v2) | set2.Remove(v1)) { --EdgeCount; return true; }
            return false;
        }
        public bool HasEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.TryGetValue(v1, out var set1) || !mapping.TryGetValue(v2, out var set2)) return false;
            if (orientedEdge) return set1.Contains(v2);
            return set1.Contains(v2) && set2.Contains(v1);
        }
        public HashSet<T>? Vertices()
        {
            if (VertexCount == 0) return null;
            HashSet<T> vertices = new();
            vertices.UnionWith(mapping.Keys);
            return vertices;
        }
        public HashSet<(T, T, bool)>? Edges()
        {
            if (EdgeCount == 0) return null;
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
