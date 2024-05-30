using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace CSharpGraphsLibrary
{
    public class Graph<T> : ITraversableGraph<T>,
                            IXmlSerializable
        where T : notnull
    {
        Dictionary<T, HashSet<T>> mapping;
        [XmlIgnore]
        public int VertexCount => mapping.Keys.Count;
        [XmlIgnore]
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
                $"Specified enumerable collection of vertices '{vertices}' was null.");
            return new Graph<T>(vertices);
        }
        public static Graph<T> Create(params T[] vertices) => new(vertices);
        public void AddVertex(T v)
        {
            if (!mapping.TryAdd(v, new())) throw new InvalidOperationException(
                $"Specified vertex '{v}' was already in the graph.");
        }
        public bool TryAddVertex(T v) => mapping.TryAdd(v, new());
        public void RemoveVertex(T v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) throw new InvalidOperationException(
                $"Specified vertex '{v}' was not in the graph.");
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
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var set2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
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
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var set2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
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
        IEnumerable<T>? ITraversableGraph<T>.Vertices() => Vertices();
        IEnumerable<T>? ITraversableGraph<T>.NeighboursOf(T vertex) => NeighboursOf(vertex);
        public static void SerializeAsXML(Graph<T> graph, string filePath)
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            using StreamWriter writer = new(filePath);
            XmlSerializer serializer = new(typeof(Graph<T>));
            serializer.Serialize(writer, graph);
        }
        public static Graph<T> DeserializeFromXML(string filePath)
        {
            using FileStream stream = new(filePath, FileMode.Open);
            XmlSerializer serializer = new(typeof(Graph<T>));
            var graph = (Graph<T>?)serializer.Deserialize(stream);
            return graph is not null ? graph : throw new InvalidOperationException(
                "Attempt to deserialize null as Graph.");
        }
        XmlSchema? IXmlSerializable.GetSchema() => null;
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Type", GetType().GetGenericTypeDefinition().Name);
            writer.WriteElementString("VertexCount", VertexCount.ToString());
            writer.WriteElementString("EdgeCount", EdgeCount.ToString());
            if (VertexCount > 0)
            {
                writer.WriteStartElement("Vertices");
                foreach (T vertex in mapping.Keys) writer.WriteElementString("V", vertex.ToString());
                writer.WriteEndElement();
            }
            if (EdgeCount > 0)
            {
                writer.WriteStartElement("Edges");
                foreach ((T edgeStart, T edgeEnd, bool oriented) in Edges()!)
                {
                    writer.WriteStartElement("E");
                    writer.WriteElementString("Start", edgeStart.ToString());
                    writer.WriteElementString("End", edgeEnd.ToString());
                    writer.WriteElementString("Oriented", oriented.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            reader.ReadStartElement();
            reader.ReadContentAsString();
            reader.ReadEndElement();
            reader.ReadStartElement();
            int vertexCount = reader.ReadContentAsInt();
            reader.ReadEndElement();
            reader.ReadStartElement();
            int edgeCount = reader.ReadContentAsInt();
            reader.ReadEndElement();
            mapping = new();
            if (vertexCount > 0)
            {
                reader.ReadStartElement();
                for (int i = 0; i < vertexCount; i++)
                {
                    reader.ReadStartElement();
                    T vertex = (T)Convert.ChangeType(reader.ReadContentAsObject(), typeof(T));
                    mapping.Add(vertex, new());
                    reader.ReadEndElement();
                }
                reader.ReadEndElement();
                if (edgeCount > 0)
                {
                    reader.ReadStartElement();
                    for (int i = 0; i < edgeCount; i++)
                    {
                        reader.ReadStartElement();
                        reader.ReadStartElement();
                        T edgeStart = (T)Convert.ChangeType(reader.ReadContentAsObject(), typeof(T));
                        reader.ReadEndElement();
                        reader.ReadStartElement();
                        T edgeEnd = (T)Convert.ChangeType(reader.ReadContentAsObject(), typeof(T));
                        reader.ReadEndElement();
                        reader.ReadStartElement();
                        string oriented = reader.ReadContentAsString();
                        bool orient = oriented == "True";
                        reader.ReadEndElement();
                        reader.ReadEndElement();
                        Connect(edgeStart, edgeEnd, orient);
                    }
                    reader.ReadEndElement();
                }
            }
            EdgeCount = edgeCount;
            reader.ReadEndElement();
        }
    }
}
