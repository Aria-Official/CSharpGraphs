using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class GraphTests
    {
        [Test]
        public void Create()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.VertexCount == 0 &&
                        graph.EdgeCount == 0 &&
                        graph.Vertices() is null &&
                        graph.Edges() is null);
        }
        [Test]
        public void CreateWithEnumerableThrowsOnNull()
        {
            IEnumerable<int> enumerable = null!;
            Assert.Throws<ArgumentNullException>(() => { var graph = Graph<int>.Create(enumerable); });
        }
        [Test]
        public void CreateWithEnumerable()
        {
            HashSet<int> vertices = new() { 1, 2, 3, 4, 5 };
            var graph = Graph<int>.Create(vertices);
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
            int[] vertices = { 1, 2, 3, 2, 1, 2, 5, 2, 3, 4, 4, 5 };
            var graph = Graph<int>.Create(vertices);
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
        public void CreateWithParams()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5);
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
            var graph = Graph<int>.Create(1, 2, 2, 3, 3, 3, 3, 3, 4, 5, 5, 5, 4);
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
            var graph = Graph<int>.Create();
            graph.AddVertex(1);
            var verts = graph.Vertices()!;
            Assert.That(graph.VertexCount == 1 && verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void AddVertexThrowsOnSame()
        {
            var graph = Graph<int>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.AddVertex(1));
        }
        [Test]
        public void TryAddVertexReturnsTrueOnNew()
        {
            var graph = Graph<int>.Create();
            bool added = graph.TryAddVertex(1);
            var verts = graph.Vertices()!;
            Assert.That(added && graph.VertexCount == 1 && verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void TryAddVertexReturnsFalseOnSame()
        {
            var graph = Graph<int>.Create(1);
            var verts = graph.Vertices()!;
            Assert.That(!graph.TryAddVertex(1) && graph.VertexCount == 1 && verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void ConnectThrowsOnFirstMissing()
        {
            var graph = Graph<int>.Create();
            Assert.Throws<InvalidOperationException>(() => graph.Connect(1, 2, true));
        }
        [Test]
        public void ConnectThrowsOnSecondMissing()
        {
            var graph = Graph<int>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.Connect(1, 2, false));
        }
        [Test]
        public void TryConnectReturnsFalseOnFirstMissing()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.TryConnect(1, 2, true), Is.False);
        }
        [Test]
        public void TryConnectReturnsFalseOnSecondMissing()
        {
            var graph = Graph<int>.Create(1);
            Assert.That(graph.TryConnect(1, 2, false), Is.False);
        }
        [Test]
        public void Connect1()
        {
            var graph = Graph<int>.Create(1, 2, 3);
            graph.Connect(1, 2, true);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(2, 1, true);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(2, 3, false);
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(2, 3, true);
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(3, 2, true);
            if (graph.EdgeCount != 2) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void Connect2()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5, 6);
            graph.Connect(1, 2, true);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Connect(1, 5, false);
            if (graph.EdgeCount != 2) Assert.Fail();
            graph.Connect(2, 6, true);
            if (graph.EdgeCount != 3) Assert.Fail();
            graph.Connect(2, 4, false);
            if (graph.EdgeCount != 4) Assert.Fail();
            graph.Connect(5, 3, true);
            if (graph.EdgeCount != 5) Assert.Fail();
            graph.Connect(4, 3, false);
            var edges = graph.Edges()!;
            if (graph.EdgeCount != 6 || edges.Count != 6 ||
                !edges.Contains((1, 2, true)) ||
                !edges.Contains((1, 5, false)) ||
                !edges.Contains((2, 6, true)) ||
                !edges.Contains((2, 4, false)) ||
                !edges.Contains((5, 3, true)) ||
                !edges.Contains((3, 4, false))) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void DisconnectThrowsOnFirstMissing()
        {
            var graph = Graph<int>.Create(1, 2);
            Assert.Throws<InvalidOperationException>(() => graph.Disconnect(3, 1));
        }
        [Test]
        public void DisconnectThrowsOnSecondMissing()
        {
            var graph = Graph<int>.Create(1, 2);
            Assert.Throws<InvalidOperationException>(() => graph.Disconnect(1, 3));
        }
        [Test]
        public void TryDisconnectReturnsFalseOnFirstMissing()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.TryDisconnect(1, 2), Is.False);
        }
        [Test]
        public void TryDisconnectReturnsFalseOnSecondMissing()
        {
            var graph = Graph<int>.Create(1);
            Assert.That(graph.TryDisconnect(1, 2), Is.False);
        }
        [Test]
        public void Disconnect()
        {
            var graph = Graph<int>.Create(1, 2);
            graph.Connect(1, 2, false);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(1, 2);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(2, 1, true);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(1, 2);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(1, 2, false);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(2, 1);
            if (graph.EdgeCount != 0) Assert.Fail();
            graph.Connect(2, 1, true);
            if (graph.EdgeCount != 1) Assert.Fail();
            graph.Disconnect(2, 1);
            if (graph.EdgeCount != 0) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void RemoveVertexThrowsOnMissing()
        {
            var graph = Graph<int>.Create();
            Assert.Throws<InvalidOperationException>(() => graph.RemoveVertex(0));
        }
        [Test]
        public void TryRemoveVertexReturnsFalseOnMissing()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.TryRemoveVertex(0), Is.False);
        }
        [Test]
        public void RemoveVertex()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5);
            graph.Connect(1, 2, true);
            graph.Connect(3, 1, true);
            graph.Connect(1, 4, false);
            graph.Connect(5, 1, false);
            if (graph.EdgeCount != 4) Assert.Fail();
            graph.RemoveVertex(1);
            if (graph.EdgeCount != 0) Assert.Fail();
            if (graph.Edges() is not null) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void HasVertexReturnsTrueOnExisting()
        {
            var graph = Graph<int>.Create(1);
            Assert.That(graph.HasVertex(1));
        }
        [Test]
        public void HasVertexReturnsFalseOnMissing()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.HasVertex(1), Is.False);
        }
        [Test]
        public void HasEdge()
        {
            var graph = Graph<int>.Create(1, 2, 3);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, false);
            Assert.That(graph.HasEdge(1, 2, true) &&
                        !graph.HasEdge(1, 2, false) &&
                        !graph.HasEdge(2, 1, true) &&
                        graph.HasEdge(1, 3, true) &&
                        graph.HasEdge(3, 1, true) &&
                        graph.HasEdge(1, 3, false) &&
                        graph.HasEdge(3, 1, false));
        }
        [Test]
        public void NeighboursOfReturnsNullOnMissingVertex()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.NeighboursOf(0) is null);
        }
        [Test]
        public void NeighboursOfOnExisting()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, true);
            graph.Connect(1, 4, false);
            IReadOnlySet<int> neighboursOf1 = graph.NeighboursOf(1)!;
            Assert.That(neighboursOf1.Contains(2) &&
                        neighboursOf1.Contains(3) &&
                        neighboursOf1.Contains(4));
        }
        [Test]
        public void VerticesReturnsNullOnEmptyGraph()
        {
            var graph = Graph<int>.Create();
            Assert.That(graph.Vertices() is null);
        }
        [Test]
        public void VerticesOnNotEmptyGraph()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            var vertices = graph.Vertices()!;
            for (int i = 0; i < 10;)
            {
                if (!vertices.Contains(i++)) Assert.Fail();
            }
            Assert.That(vertices.Count == 10);
        }
        [Test]
        public void EdgesReturnsNullOnNoEdgesGraph()
        {
            var graph = Graph<int>.Create(0, 1, 2);
            Assert.That(graph.Edges() is null);
        }
        [Test]
        public void EdgesOnGraphWithEdges()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, true);
            graph.Connect(1, 4, false);
            graph.Connect(2, 1, true);
            var edges = graph.Edges()!;
            Assert.That(edges.Contains((1, 3, true)) &&
                        edges.Contains((1, 4, false)) &&
                        edges.Contains((2, 1, true)) &&
                        edges.Count == 3);
        }
    }
}
