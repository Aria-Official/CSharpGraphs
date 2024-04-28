namespace CSharpGraphsLibrary
{
    public class Graph<T> where T : notnull
    {
        readonly Dictionary<T, HashSet<T>> mapping;
        public int VertexCount { get; private set; }
        public int EdgeCount { get; private set; }
        Graph()
        {
            mapping = new();
            VertexCount = 0;
            EdgeCount = 0;
        }
        Graph(IEnumerable<T> vertices) : this()
        {
            foreach (T v in vertices)
            {
                mapping.Add(v, new());
                ++VertexCount;
            }
        }
        Graph(T[] vertices)
        {
            mapping = new();
            foreach (T v in vertices) mapping.Add(v, new());
            VertexCount = vertices.Length;
            EdgeCount = 0;
        }
        public static Graph<T> Create() => new();
        public static Graph<T> Create(IEnumerable<T> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                "Create() failed. Specified enumerable collection of vertices was null.");
            return new Graph<T>(vertices);
        }
        public static Graph<T> Create(params T[] vertices) => new(vertices);
        public void AddVertex(T v)
        {
            if (mapping.ContainsKey(v)) throw new InvalidOperationException(
                "AddVertex() failed. Specified vertex was already in the graph.");
            mapping.Add(v, new());
        }
        public bool TryAddVertex(T v) => mapping.TryAdd(v, new());
        public void RemoveVertex(T v)
        {
            if (mapping.Remove(v))
            {
                foreach (T other in mapping.Keys) mapping[other].Remove(v);
                return;
            }
            throw new InvalidOperationException(
                "RemoveVertex() failed. Specified vertex was not in the graph");
        }
        public bool TryRemoveVertex(T v)
        {
            if (mapping.Remove(v))
            {
                foreach (T other in mapping.Keys) mapping[other].Remove(v);
                return true;
            }
            return false;
        }
        public bool HasVertex(T vertex) => mapping.ContainsKey(vertex);
        public void AddEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"AddEdge() failed. Vertex {v1} was not in the graph");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"AddEdge() failed. Vertex {v2} was not in the graph");
            if (orientedEdge)
            {
                if (mapping[v1].Contains(v2)) return;
                mapping[v1].Add(v2);
            }
            else
            {
                if (!mapping[v1].Contains(v2)) mapping[v1].Add(v2);
                if (mapping[v2].Contains(v1)) return;
                mapping[v2].Add(v1);
            }
        }
        public void RemoveEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1)) throw new InvalidOperationException(
                $"RemoveEdge() failed. Vertex {v1} was not in the graph");
            if (!mapping.ContainsKey(v2)) throw new InvalidOperationException(
                $"RemoveEdge() failed. Vertex {v2} was not in the graph");
            if (orientedEdge) mapping[v1].Remove(v2);
            else
            {
                mapping[v1].Remove(v2);
                mapping[v2].Remove(v1);
            }
        }
        public bool HasEdge(T v1, T v2, bool orientedEdge)
        {
            if (!mapping.ContainsKey(v1) || !mapping.ContainsKey(v2)) return false;
            if (orientedEdge) return mapping[v1].Contains(v2);
            return mapping[v1].Contains(v2) & mapping[v2].Contains(v1);
        }
        public IEnumerable<T> Vertices()
        {
            foreach (T v in mapping.Keys) yield return v;
        }
        /*public IEnumerable<(T, T)> Edges()
        {

        }*/
    }
}
