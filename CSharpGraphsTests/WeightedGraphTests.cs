using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class WeightedGraphTests
    {
        [Test]
        public void Create()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.VertexCount == 0 &&
                        graph.EdgeCount == 0 &&
                        graph.Vertices() is null &&
                        graph.Edges() is null);
        }
        [Test]
        public void CreateWithEnumerableThrowsOnNull()
        {
            IEnumerable<int> enumerable = null!;
            Assert.Throws<ArgumentNullException>(() =>
            { var graph = WeightedGraph<int, int>.Create(enumerable); });
        }
        [Test]
        public void CreateWithEnumerable()
        {
            HashSet<int> vertices = new() { 1, 2, 3, 4, 5 };
            var graph = WeightedGraph<int, int>.Create(vertices);
            var verts = graph.Vertices()!;
            Assert.That(graph.EdgeCount == 0 &&
                        graph.VertexCount == 5 &&
                        verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        graph.Edges() is null);
        }
        [Test]
        public void CreateWithEnumerableWithSameVertices()
        {
            HashSet<int> vertices = new() { 1, 2, 3, 2, 1, 2, 5, 2, 3, 4, 4, 5 };
            var graph = WeightedGraph<int, int>.Create(vertices);
            var verts = graph.Vertices()!;
            Assert.That(graph.EdgeCount == 0 &&
                        graph.VertexCount == 5 &&
                        verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        graph.Edges() is null);
        }
        [Test]
        public void CreateWithParams()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5);
            var verts = graph.Vertices()!;
            Assert.That(graph.VertexCount == 5 &&
                        verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        graph.Edges() is null);
        }
        [Test]
        public void CreateWithParamsWithSameVertices()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 2, 3, 3, 3, 3, 3, 4, 5, 5, 5, 4);
            var verts = graph.Vertices()!;
            Assert.That(graph.VertexCount == 5 &&
                        verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        graph.Edges() is null);
        }
        [Test]
        public void AddVertexAddsNew()
        {
            var graph = WeightedGraph<int, int>.Create();
            graph.AddVertex(1);
            var verts = graph.Vertices()!;
            Assert.That(graph.VertexCount == 1 && verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void AddVertexThrowsOnSame()
        {
            var graph = WeightedGraph<int, short>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.AddVertex(1));
        }
        [Test]
        public void TryAddVertexReturnsTrueOnNew()
        {
            var graph = WeightedGraph<int, double>.Create();
            bool added = graph.TryAddVertex(1);
            var verts = graph.Vertices()!;
            Assert.That(added && graph.VertexCount == 1 &&
                                 verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void TryAddVertexReturnsFalseOnSame()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            var verts = graph.Vertices()!;
            Assert.That(!graph.TryAddVertex(1) &&
                        graph.VertexCount == 1 &&
                        verts.Count == 1 &&
                        verts.Contains(1));
        }
        [Test]
        public void ConnectThrowsOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.Throws<InvalidOperationException>(() => graph.Connect(1, 2, false, 10));
        }
        [Test]
        public void ConnectThrowsOnSecondMissing()
        {
            var graph = WeightedGraph<int, char>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.Connect(1, 2, false, 'A'));
        }
        [Test]
        public void TryConnectReturnsFalseOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.TryConnect(1, 2, true, 10), Is.False);
        }
        [Test]
        public void TryConnectReturnsFalseOnSecondMissing()
        {
            var graph = WeightedGraph<int, string>.Create(1);
            Assert.That(graph.TryConnect(1, 2, false, "Edge"), Is.False);
        }
        [Test]
        public void Connect1()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3);
            graph.Connect(1, 2, true, 10);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(2, 1, true, 5);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(2, 3, false, 6);
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(2, 3, true, 7);
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(3, 2, true, 3);
            if (graph.EdgeCount != 2) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void Connect2()
        {
            var graph = WeightedGraph<int, char>.Create(1, 2, 3, 4, 5, 6);
            graph.Connect(1, 2, true, 'A');
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(1, 5, false, 'B');
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(2, 6, true, 'C');
            if (graph.EdgeCount != 3) Assert.Fail();
            graph.Connect(2, 4, false, 'D');
            if (graph.EdgeCount != 4) Assert.Fail();
            graph.Connect(5, 3, true, 'E');
            if (graph.EdgeCount != 5) Assert.Fail();
            graph.Connect(4, 3, false, 'F');
            var edges = graph.Edges()!;
            if (graph.EdgeCount != 6 || edges.Count != 6 ||
                !edges.Contains((1, 2, true, 'A')) ||
                !edges.Contains((1, 5, false, 'B')) ||
                !edges.Contains((2, 6, true, 'C')) ||
                !edges.Contains((2, 4, false, 'D')) ||
                !edges.Contains((5, 3, true, 'E')) ||
                !edges.Contains((3, 4, false, 'F'))) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void TryConnect1()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3);
            bool con = graph.TryConnect(1, 2, true, 10);
            if (graph.EdgeCount != 1 || !con) Assert.Fail();
            con = graph.TryConnect(2, 1, true, 5);
            if (graph.EdgeCount != 1 || !con) Assert.Fail();
            con = graph.TryConnect(2, 3, false, 6);
            if (graph.EdgeCount != 2 || !con) Assert.Fail();
            con = graph.TryConnect(2, 3, true, 7);
            if (graph.EdgeCount != 2 || !con) Assert.Fail();
            con = graph.TryConnect(3, 2, true, 3);
            if (graph.EdgeCount != 2 || !con) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void TryConnect2()
        {
            var graph = WeightedGraph<int, char>.Create(1, 2, 3, 4, 5, 6);
            bool con = graph.TryConnect(1, 2, true, 'A');
            if (graph.EdgeCount != 1 || !con) Assert.Fail();
            con = graph.TryConnect(1, 5, false, 'B');
            if (graph.EdgeCount != 2 || !con) Assert.Fail();
            con = graph.TryConnect(2, 6, true, 'C');
            if (graph.EdgeCount != 3 || !con) Assert.Fail();
            con = graph.TryConnect(2, 4, false, 'D');
            if (graph.EdgeCount != 4 || !con) Assert.Fail();
            con = graph.TryConnect(5, 3, true, 'E');
            if (graph.EdgeCount != 5 || !con) Assert.Fail();
            con = graph.TryConnect(4, 3, false, 'F');
            var edges = graph.Edges()!;
            if (graph.EdgeCount != 6 || edges.Count != 6 || !con ||
                !edges.Contains((1, 2, true, 'A')) ||
                !edges.Contains((1, 5, false, 'B')) ||
                !edges.Contains((2, 6, true, 'C')) ||
                !edges.Contains((2, 4, false, 'D')) ||
                !edges.Contains((5, 3, true, 'E')) ||
                !edges.Contains((3, 4, false, 'F'))) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void DisconnectThrowsOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.Throws<InvalidOperationException>(() => graph.Disconnect(1, 2));
        }
        [Test]
        public void DisconnectThrowsOnSecondMissing()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.Disconnect(1, 2));
        }
        [Test]
        public void TryDisconnectReturnsFalseOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.TryDisconnect(1, 2), Is.False);
        }
        [Test]
        public void TryDisconnectReturnsFalseOnSecondMissing()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            Assert.That(graph.TryDisconnect(1, 2), Is.False);
        }
        [Test]
        public void Disconnect()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2);
            graph.Connect(1, 2, false, 1);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(1, 2);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(2, 1, true, 2);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(1, 2);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(1, 2, false, 3);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(2, 1);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(2, 1, true, 4);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(2, 1);
            if (graph.EdgeCount != 0) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void TryDisconnect()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2);
            graph.Connect(1, 2, false, 1);
            if (graph.EdgeCount != 1) Assert.Fail();
            bool discon = graph.TryDisconnect(1, 2);
            if (graph.EdgeCount != 0 && discon) Assert.Fail();
            graph.Connect(2, 1, true, 2);
            if (graph.EdgeCount != 1) Assert.Fail();
            discon = graph.TryDisconnect(1, 2);
            if (graph.EdgeCount != 0 && discon) Assert.Fail();
            graph.Connect(1, 2, false, 3);
            if (graph.EdgeCount != 1) Assert.Fail();
            discon = graph.TryDisconnect(2, 1);
            if (graph.EdgeCount != 0 && discon) Assert.Fail();
            graph.Connect(2, 1, true, 4);
            if (graph.EdgeCount != 1) Assert.Fail();
            discon = graph.TryDisconnect(2, 1);
            if (graph.EdgeCount != 0 && discon) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void TryDisconnectReturnsFalseOnMissingEdge()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2);
            Assert.That(graph.TryDisconnect(1, 2), Is.False);
        }
        [Test]
        public void RemoveVertexThrowsOnMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.Throws<InvalidOperationException>(() => graph.RemoveVertex(0));
        }
        [Test]
        public void TryRemoveVertexReturnsFalseOnMissing()
        {
            var graph = WeightedGraph<int, char>.Create();
            Assert.That(graph.TryRemoveVertex(0), Is.False);
        }
        [Test]
        public void RemoveVertex()
        {
            var graph = WeightedGraph<int, double>.Create(1, 2, 3, 4, 5);
            graph.Connect(1, 2, true, .1);
            graph.Connect(3, 1, true, .2);
            graph.Connect(1, 4, false, .3);
            graph.Connect(5, 1, false, .4);
            if (graph.EdgeCount != 4) Assert.Fail();
            graph.RemoveVertex(1);
            if (graph.EdgeCount != 0) Assert.Fail();
            if (graph.Edges() is not null) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void HasVertexReturnsTrueOnExisting()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            Assert.That(graph.HasVertex(1));
        }
        [Test]
        public void HasVertexReturnsFalseOnMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.HasVertex(1), Is.False);
        }
        [Test]
        public void HasEdgeReturnsFalseOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.HasEdge(1, 2, true), Is.False);
        }
        [Test]
        public void HasEdgeReturnsFalseOnSecondMissing()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            Assert.That(graph.HasEdge(1, 2, true), Is.False);
        }
        [Test]
        public void HasEdgeWithEdgeWeightOutReturnsFalseOnFirstMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.HasEdge(1, 2, true, out int _), Is.False);
        }
        [Test]
        public void HasEdgeWithEdgeWeightOutReturnsFalseOnSecondMissing()
        {
            var graph = WeightedGraph<int, int>.Create(1);
            Assert.That(graph.HasEdge(1, 2, true, out int _), Is.False);
        }
        [Test]
        public void HasEdge()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3);
            graph.Connect(1, 2, true, 1);
            graph.Connect(1, 3, false, 2);
            Assert.That(graph.HasEdge(1, 2, true) &&
                        !graph.HasEdge(1, 2, false) &&
                        !graph.HasEdge(2, 1, true) &&
                        graph.HasEdge(1, 3, true) &&
                        graph.HasEdge(3, 1, true) &&
                        graph.HasEdge(1, 3, false) &&
                        graph.HasEdge(3, 1, false));
        }
        [Test]
        public void HasEdgeWithEdgeWeightOut()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3);
            graph.Connect(1, 2, true, 1);
            graph.Connect(1, 3, false, 2);
            bool hasEdge12 = graph.HasEdge(1, 2, true, out int weight12),
                 hasEdge13 = graph.HasEdge(1, 3, false, out int weight13);
            Assert.That(hasEdge12 && hasEdge13 &&
                        weight12 == 1 && weight13 == 2 &&
                        !graph.HasEdge(1, 2, false) &&
                        !graph.HasEdge(2, 1, true) &&
                        graph.HasEdge(1, 3, true) &&
                        graph.HasEdge(3, 1, true) &&
                        graph.HasEdge(1, 3, false) &&
                        graph.HasEdge(3, 1, false));
        }
        [Test]
        public void HasEdgeWithEdgeWeightOutOutsDefaultEdgeWeightOnMissingEdge()
        {
            var graph1 = WeightedGraph<int, int>.Create(1, 2);
            var graph2 = WeightedGraph<int, string>.Create(1, 2);
            bool hasEdge1 = graph1.HasEdge(1, 2, true, out int weight1),
                 hasEdge2 = graph2.HasEdge(1, 2, true, out string? weight2);
            Assert.That(!hasEdge1 && !hasEdge2 && weight1 == 0 && weight2 is null);
        }
        [Test]
        public void NeighboursOfReturnsNullOnMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.NeighboursOf(0) is null);
        }
        [Test]
        public void NeighboursOfOnExisting()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true, 1);
            graph.Connect(1, 3, true, 2);
            graph.Connect(1, 4, false, 3);
            IEnumerable<int> neighboursOf1 = graph.NeighboursOf(1)!;
            Assert.That(neighboursOf1.Contains(2) &&
                        neighboursOf1.Contains(3) &&
                        neighboursOf1.Contains(4));
        }
        [Test]
        public void NeighboursWithWeightOfRetunsNullOnMissing()
        {
            var graph = WeightedGraph<int, int>.Create();
            Assert.That(graph.NeighboursWithWeightOf(0) is null);
        }
        [Test]
        public void NeighboursWithWeightOfOnExisting()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true, 1);
            graph.Connect(1, 3, true, 2);
            graph.Connect(1, 4, false, 3);
            IReadOnlyDictionary<int, int> neighboursOf1 = graph.NeighboursWithWeightOf(1)!;
            bool has2 = neighboursOf1.TryGetValue(2, out int weightTo2),
                 has3 = neighboursOf1.TryGetValue(3, out int weightTo3),
                 has4 = neighboursOf1.TryGetValue(4, out int weightTo4);
            Assert.That(has2 && has3 && has4 &&
                        weightTo2 == 1 && weightTo3 == 2 && weightTo4 == 3);
        }
        [Test]
        public void VerticesReturnsNullOnEmptyGraph()
        {
            var graph = WeightedGraph<double, int>.Create();
            Assert.That(graph.Vertices() is null);
        }
        [Test]
        public void VerticesOnNotEmptyGraph()
        {
            var graph = WeightedGraph<int, double>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            var vertices = graph.Vertices()!;
            for (int i = 0; i < 1;)
            {
                if (!vertices.Contains(i++)) Assert.Fail();
            }
            Assert.That(vertices.Count == 10);
        }
        [Test]
        public void EdgesReturnsNullOnNoEdgesGraph()
        {
            var graph = WeightedGraph<char, int>.Create('A', 'B', 'C');
            Assert.That(graph.Edges() is null);
        }
        [Test]
        public void EdgesOnGraphWithEdges()
        {
            var graph = WeightedGraph<char, int>.Create('A', 'B', 'C', 'D');
            graph.Connect('A', 'B', true, 1);
            graph.Connect('A', 'C', true, 2);
            graph.Connect('A', 'D', false, 3);
            graph.Connect('B', 'A', true, 4);
            var edges = graph.Edges()!;
            Assert.That(edges.Contains(('A', 'C', true, 2)) &&
                        edges.Contains(('A', 'D', false, 3)) &&
                        edges.Contains(('B', 'A', true, 4)) &&
                        edges.Count == 3);
        }
        [Test]
        public void SerializeAsXMLThrowsOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => WeightedGraph<int, int>.SerializeAsXML(null!, ""));
        }
        [Test]
        public void SerializationTestForEmptyWeightedGraph()
        {
            var graph = WeightedGraph<double, double>.Create();
            string path = Path.Combine(@"C:\Users\aria17\Desktop\WeightedGraphSerializationTests",
                                        "SerializationTestForEmptyWeightedGraph.xml");
            WeightedGraph<double, double>.SerializeAsXML(graph, path);
            var deserialized = WeightedGraph<double, double>.DeserializeFromXML(path);
            Assert.That(deserialized.VertexCount == 0 &&
                        deserialized.EdgeCount == 0);
        }
        [Test]
        public void SerializationTestForNoEdgesWeightedGraph()
        {
            var graph = WeightedGraph<char, int>.Create('A', 'B', 'C', 'D');
            string path = Path.Combine(@"C:\Users\aria17\Desktop\WeightedGraphSerializationTests",
                                        "SerializationTestForNoEdgesWeightedGraph.xml");
            WeightedGraph<char, int>.SerializeAsXML(graph, path);
            var deserialized = WeightedGraph<char, int>.DeserializeFromXML(path);
            for (int i = 65; i < 69;) if (!deserialized.HasVertex((char)(i++))) Assert.Fail();
            Assert.That(deserialized.VertexCount == 4 &&
                        deserialized.EdgeCount == 0);
        }
        [Test]
        public void SerializationTest()
        {
            var graph = WeightedGraph<int, double>.Create(1, 2, 3, 4, 5);
            graph.Connect(1, 2, true, 10.1);
            graph.Connect(2, 3, false, 20.2);
            graph.Connect(3, 1, true, 30.3);
            graph.Connect(3, 4, true, 40.4);
            graph.Connect(1, 5, false, 50.5);
            string path = Path.Combine(@"C:\Users\aria17\Desktop\WeightedGraphSerializationTests",
                                        "SerializationTest.xml");
            WeightedGraph<int, double>.SerializeAsXML(graph, path);
            var deserialized = WeightedGraph<int, double>.DeserializeFromXML(path);
            for (int i = 1; i < 6;) if (!deserialized.HasVertex(i++)) Assert.Fail();
            IEnumerable<(int, int, bool, double)> edges = deserialized.Edges()!;
            Assert.That(deserialized.VertexCount == 5 &&
                        deserialized.EdgeCount == 5 &&
                        edges.Contains((1, 2, true, 10.1)) &&
                        edges.Contains((2, 3, false, 20.2)) &&
                        edges.Contains((3, 1, true, 30.3)) &&
                        edges.Contains((3, 4, true, 40.4)) &&
                        edges.Contains((1, 5, false, 50.5)));
        }
    }
}
