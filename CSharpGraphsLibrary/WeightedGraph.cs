using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace CSharpGraphsLibrary
{
    /// <summary>
    /// Represents implicitly oriented weighted graph.
    /// </summary>
    /// <typeparam name="TVertex">Type of graph vertex.</typeparam>
    /// <typeparam name="TEdgeWeight">Type of graph edge weight.</typeparam>
    public class WeightedGraph<TVertex, TEdgeWeight> : ITraversableGraph<TVertex>,
                                                       IXmlSerializable
        where TVertex : notnull
    {
        Dictionary<TVertex, Dictionary<TVertex, TEdgeWeight>> mapping;
        /// <summary>
        /// Read-only amount of vertices in the graph.
        /// </summary>
        [XmlIgnore]
        public int VertexCount => mapping.Keys.Count;
        /// <summary>
        /// Read-only amount of edges in the graph.
        /// </summary>
        [XmlIgnore]
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
        /// <summary>
        /// Creates weighted graph with no vertices.
        /// </summary>
        /// <returns>Weighted graph with no vertices.</returns>
        public static WeightedGraph<TVertex, TEdgeWeight> Create() => new();
        /// <summary>
        /// Creates weighted graph with vertices from specified enumerable collection.
        /// </summary>
        /// <param name="vertices">Enumerable collection of vertices.</param>
        /// <returns>Weighted graph containing vertices from specified enumerable collection.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when specified enumerable collection of vertices is <see langword="null"/>.</exception>
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IEnumerable<TVertex> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                $"Specified enumerable collection of vertices '{vertices}' was null.");
            return new WeightedGraph<TVertex, TEdgeWeight>(vertices);
        }
        /// <summary>
        /// Creates weighted graph with vertices enumerated in method arguments.
        /// </summary>
        /// <param name="vertices">Vertices enumerated in method arguments.</param>
        /// <returns>Weighted graph containing vertices enumerated in method arguments.</returns>
        public static WeightedGraph<TVertex, TEdgeWeight> Create(params TVertex[] vertices) => new(vertices);
        /// <summary>
        /// Adds specified vertex to the graph.
        /// </summary>
        /// <param name="v">Vertex to add in the graph.</param>
        /// <exception cref="InvalidOperationException">Is thrown when specified vertex is already presented in the graph.</exception>
        public void AddVertex(TVertex v)
        {
            if (!mapping.TryAdd(v, new())) throw new InvalidOperationException(
                $"Specified vertex '{v}' was already in the graph.");
        }
        /// <summary>
        /// Tries to add specified vertex in the graph.
        /// </summary>
        /// <param name="v">Vertex to add in the graph.</param>
        /// <returns><see langword="true"/> if vertex was successfully added in the graph; otherwise, <see langword="false"/>.</returns>
        public bool TryAddVertex(TVertex v) => mapping.TryAdd(v, new());
        /// <summary>
        /// Removes specifed vertex from the graph deleting adjacent edges.
        /// </summary>
        /// <param name="v">Vertex to remove from the graph.</param>
        /// <exception cref="InvalidOperationException">Is thrown when specified vertex was not in the graph.</exception>
        public void RemoveVertex(TVertex v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) throw new InvalidOperationException(
                $"Specified vertex '{v}' was not in the graph.");
            foreach ((TVertex pointed, TEdgeWeight _) in neighbours) Disconnect(v, pointed);
            foreach (TVertex pointing in mapping.Keys) Disconnect(pointing, v);
            mapping.Remove(v);
        }
        /// <summary>
        /// Tries to remove specifed vertex from the graph deleting adjacent edges.
        /// </summary>
        /// <param name="v">Vertex to remove from the graph.</param>
        /// <returns><see langword="true"/> if vertex was successfully removed from the graph; otherwise, <see langword="false"/>.</returns>
        public bool TryRemoveVertex(TVertex v)
        {
            if (!mapping.TryGetValue(v, out var neighbours)) return false;
            foreach ((TVertex pointed, TEdgeWeight _) in neighbours) Disconnect(v, pointed);
            foreach (TVertex pointing in mapping.Keys) Disconnect(pointing, v);
            return mapping.Remove(v); // This line is designed to always return true.
        }
        /// <summary>
        /// Checks whether specified vertex is in the graph.
        /// </summary>
        /// <param name="v">Vertex to check for being in the graph.</param>
        /// <returns><see langword="true"/> if vertex is in the graph; otherwise, <see langword="false"/>.</returns>
        public bool HasVertex(TVertex v) => mapping.ContainsKey(v);
        /// <summary>
        /// Gets enumerable collection of neighbour vertices of the specified graph vertex.
        /// </summary>
        /// <param name="v">Vertex to get neighbour vertices of.</param>
        /// <returns>Enumerable collection of neighbour vertices of the vertex if vertex is in the graph; otherwise, <see langword="null"/>.</returns>
        public IEnumerable<TVertex>? NeighboursOf(TVertex v)
        {
            bool found = mapping.TryGetValue(v, out var neighbours);
            if (found) return neighbours!.Keys;
            return null;
        }
        /// <summary>
        /// Get read-only dictionary of pairs "(neighbour vertex; weight of edge to this vertex)" of the specified graph vertex.
        /// </summary>
        /// <param name="v">Vertex to get neighbour vertices with weight of edges to them of.</param>
        /// <returns>Read-only dictionary of pairs "(neighbour vertex; weight of edge to this vertex)" of the vertex if vertex is in the graph;
        /// otherwise, <see langword="null"/>.</returns>
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
        /// <summary>
        /// Connects two specified vertices with an edge of specified weight.
        /// </summary>
        /// <param name="v1">Vertex to be edge start.</param>
        /// <param name="v2">Vertex to be edge end.</param>
        /// <param name="orientedEdge">Value indicating whether edge must be oriented.</param>
        /// <param name="edgeWeight">Weight of the edge to be created.</param>
        /// <exception cref="InvalidOperationException">Is thrown when at least one of specified vertices is not in the graph.</exception>
        public void Connect(TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) throw new InvalidOperationException(
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var dict2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
            Connect(v1, v2, dict1, dict2, orientedEdge, edgeWeight);
        }
        /// <summary>
        /// Tries to connect two specified vertices with an edge of specified weight.
        /// </summary>
        /// <param name="v1">Vertex to be edge start.</param>
        /// <param name="v2">Vertex to be edge end.</param>
        /// <param name="orientedEdge">Value indicating whether edge must be oriented.</param>
        /// <param name="edgeWeight">Weight of the edge to be created.</param>
        /// <returns><see langword="true"/> if vertices were successfully connected; otherwise, <see langword="false"/>.</returns>
        public bool TryConnect(TVertex v1, TVertex v2, bool orientedEdge, TEdgeWeight edgeWeight)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) return false;
            if (!mapping.TryGetValue(v2, out var dict2)) return false;
            Connect(v1, v2, dict1, dict2, orientedEdge, edgeWeight);
            return true;
        }
        /// <summary>
        /// Disconnects two specified vertices removing the edge between them.
        /// </summary>
        /// <param name="v1">First end of the edge.</param>
        /// <param name="v2">Second end of the edge.</param>
        /// <exception cref="InvalidOperationException">Is thrown when at least one of specified vertices is not in the graph.</exception>
        public void Disconnect(TVertex v1, TVertex v2)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) throw new InvalidOperationException(
                $"Specified vertex '{v1}' was not in the graph.");
            if (!mapping.TryGetValue(v2, out var dict2)) throw new InvalidOperationException(
                $"Specified vertex '{v2}' was not in the graph.");
            if (dict1.Remove(v2) | dict2.Remove(v1)) --EdgeCount;
        }
        /// <summary>
        /// Tries to disconnect two specified vertices removing the edge between them.
        /// </summary>
        /// <param name="v1">First end of the edge.</param>
        /// <param name="v2">Second end of the edge.</param>
        /// <returns><see langword="true"/> if vertices were successfully disconnected; otherwise, <see langword="false"/>.</returns>
        public bool TryDisconnect(TVertex v1, TVertex v2)
        {
            if (!mapping.TryGetValue(v1, out var dict1)) return false;
            if (!mapping.TryGetValue(v2, out var dict2)) return false;
            if (dict1.Remove(v2) | dict2.Remove(v1)) { --EdgeCount; return true; }
            return false;
        }
        /// <summary>
        /// Checks whether graph contains the edge with two specified vertices being its ends.
        /// </summary>
        /// <param name="v1">Vertex being edge start.</param>
        /// <param name="v2">Vertex being edge end.</param>
        /// <param name="orientedEdge">Value indicating whether only oriented edge must be considered.</param>
        /// <returns><see langword="true"/> if edge was successfully found; otherwise, <see langword="false"/>.</returns>
        public bool HasEdge(TVertex v1, TVertex v2, bool orientedEdge)
        {
            if (!mapping.TryGetValue(v1, out var dict1) || !mapping.TryGetValue(v2, out var dict2)) return false;
            if (orientedEdge) return dict1.ContainsKey(v2);
            return dict1.ContainsKey(v2) & dict2.ContainsKey(v1);
        }
        /// <summary>
        /// Checks whether graph contains the edge with two specified vertices being its ends and outs weight of this edge.
        /// </summary>
        /// <param name="v1">Vertex being edge start.</param>
        /// <param name="v2">Vertex being edge end.</param>
        /// <param name="orientedEdge">Value indicating whether only oriented edge must be considered.</param>
        /// <param name="edgeWeight">Weight of the edge if edge was successfully found; otherwise, default value of the type.</param>
        /// <returns><see langword="true"/> if edge was successfully found; otherwise, <see langword="false"/>.</returns>
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
        /// <summary>
        /// Returns set of graph vertices.
        /// </summary>
        /// <returns>Set of graph vertices if there are any; otherwise, <see langword="null"/>.</returns>
        public HashSet<TVertex>? Vertices()
        {
            if (VertexCount == 0) return null;
            HashSet<TVertex> vertices = new();
            vertices.UnionWith(mapping.Keys);
            return vertices;
        }
        /// <summary>
        /// Returns set of graph edges.
        /// </summary>
        /// <returns>Set of graph edges if there are any; otherwise, <see langword="null"/>.</returns>
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
        IEnumerable<TVertex>? ITraversableGraph<TVertex>.Vertices() => Vertices();
        /// <summary>
        /// Serializes specified weighted graph as XML file located by specified path.
        /// </summary>
        /// <param name="graph">Weighted graph to serialize.</param>
        /// <param name="filePath">Path of file to serialize weighted graph into.</param>
        /// <exception cref="ArgumentNullException">Is thrown when specified weighted graph is <see langword="null"/>.</exception>
        public static void SerializeAsXML(WeightedGraph<TVertex, TEdgeWeight> graph, string filePath)
        {
            if (graph is null) throw new ArgumentNullException($"Specified graph '{graph}' was null.");
            using StreamWriter writer = new(filePath);
            XmlSerializer serializer = new(typeof(WeightedGraph<TVertex, TEdgeWeight>));
            serializer.Serialize(writer, graph);
        }
        /// <summary>
        /// Deserializes weighted graph from XML file located by specified path.
        /// </summary>
        /// <param name="filePath">Path of file to deserialize weighted graph from.</param>
        /// <returns>Weighted graph that is deserialized from XML file.</returns>
        /// <exception cref="InvalidOperationException">Is thrown when deserialization result is <see langword="null"/>.</exception>
        public static WeightedGraph<TVertex, TEdgeWeight> DeserializeFromXML(string filePath)
        {
            using FileStream stream = new(filePath, FileMode.Open);
            XmlSerializer serializer = new(typeof(WeightedGraph<TVertex, TEdgeWeight>));
            var graph = (WeightedGraph<TVertex, TEdgeWeight>?)serializer.Deserialize(stream);
            return graph is not null ? graph : throw new InvalidOperationException(
                "Attempt to deserialize null as WeightedGraph.");
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
                foreach (TVertex vertex in mapping.Keys) writer.WriteElementString("V", vertex.ToString());
                writer.WriteEndElement();
            }
            if (EdgeCount > 0)
            {
                writer.WriteStartElement("Edges");
                foreach ((TVertex edgeStart, TVertex edgeEnd, bool oriented, TEdgeWeight weight) in Edges()!)
                {
                    writer.WriteStartElement("E");
                    writer.WriteElementString("Start", edgeStart.ToString());
                    writer.WriteElementString("End", edgeEnd.ToString());
                    writer.WriteElementString("Oriented", oriented.ToString());
                    writer.WriteElementString("Weight", weight!.ToString());
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
                    TVertex vertex = (TVertex)Convert.ChangeType(reader.ReadContentAsObject(), typeof(TVertex));
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
                        TVertex edgeStart = (TVertex)Convert.ChangeType(reader.ReadContentAsObject(), typeof(TVertex));
                        reader.ReadEndElement();
                        reader.ReadStartElement();
                        TVertex edgeEnd = (TVertex)Convert.ChangeType(reader.ReadContentAsObject(), typeof(TVertex));
                        reader.ReadEndElement();
                        reader.ReadStartElement();
                        string oriented = reader.ReadContentAsString();
                        bool orient = oriented == "True";
                        reader.ReadEndElement();
                        reader.ReadStartElement();
                        TEdgeWeight weight = (TEdgeWeight)Convert.ChangeType(reader.ReadContentAsObject(), typeof(TEdgeWeight));   
                        reader.ReadEndElement();
                        reader.ReadEndElement();
                        Connect(edgeStart, edgeEnd, orient, weight);
                    }
                    reader.ReadEndElement();
                }
            }
            EdgeCount = edgeCount;
            reader.ReadEndElement();
        }
    }
}
