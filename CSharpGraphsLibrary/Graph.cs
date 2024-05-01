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
            foreach (T v in vertices) mapping.Add(v, new());
        }
        public static Graph<T> Create() => new();
        public static Graph<T> Create(IEnumerable<T> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                "Specified enumerable collection of vertices was null.");
            return new Graph<T>(vertices);
        }
        public static Graph<T> Create(params T[] vertices) => new(vertices);
        public void AddVertex(T v)
        {
            if (mapping.ContainsKey(v)) throw new InvalidOperationException(
                "Specified vertex was already in the graph.");
            mapping.Add(v, new());
        }
        public void RemoveVertex(T v)
        {
            if (!mapping.ContainsKey(v)) throw new InvalidOperationException(
                "Specified vertex was not in the graph.");
            foreach (T neighbourForward in mapping[v]) Disconnect(v, neighbourForward, false);
            foreach (T neighbourBackward in mapping.Keys) Disconnect(neighbourBackward, v, true);
            mapping.Remove(v);
        }
        public bool HasVertex(T vertex) => mapping.ContainsKey(vertex);
        public void Connect(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Vertex {v1} was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Vertex {v2} was not in the graph.");
            if (orientedEdge)
            {
                if (mapping[v1].Add(v2) && !mapping[v2].Contains(v1)) ++EdgeCount;
            }
            else
            {
                if (mapping[v1].Add(v2) & mapping[v2].Add(v1)) ++EdgeCount;
            }
        }
        public void Disconnect(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"Vertex {v1} was not in the graph.");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"Vertex {v2} was not in the graph.");
            if (orientedEdge)
            {
                if (mapping[v1].Remove(v2) && !mapping[v2].Contains(v1)) --EdgeCount;
            }
            else
            {
                bool forward = mapping[v1].Remove(v2), backward = mapping[v2].Remove(v1);
                if (forward)
                {
                    if (backward || !mapping[v2].Contains(v1)) --EdgeCount;
                }
                else if (backward) --EdgeCount;
            }
        }
        public bool HasEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1) || !mapping.ContainsKey(v2)) return false;
            if (orientedEdge) return mapping[v1].Contains(v2);
            return mapping[v1].Contains(v2) & mapping[v2].Contains(v1);
        }
        public HashSet<T> NeighboursOf(T v)
        {
            if (!mapping.ContainsKey(v)) throw new InvalidOperationException(
                $"Vertex {v} was not in the graph.");
            return mapping[v];
        }
        public IEnumerable<T> Vertices()
        {
            foreach (T v in mapping.Keys) yield return v;
        }
        public IEnumerable<(T, T, bool)> Edges()
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
