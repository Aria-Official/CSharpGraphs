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
            Assert.That(graph.EdgeCount == 0 &
                        graph.Vertices().Count == 0 &
                        !graph.Edges().Any());
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
            HashSet<int> verts = graph.Vertices();
            Assert.That(graph.EdgeCount == 0 &
                        verts.Count == 5 &
                        verts.Contains(1) &
                        verts.Contains(2) &
                        verts.Contains(3) &
                        verts.Contains(4) &
                        verts.Contains(5) &
                        !graph.Edges().Any());
        }
        [Test]
        public void CreateWithEnumerableWithSameVertices()
        {
            int[] vertices = { 1, 2, 3, 2, 1, 2, 5, 2, 3, 4, 4, 5 };
            var graph = Graph<int>.Create(vertices);
            HashSet<int> verts = graph.Vertices();
            Assert.That(verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        !graph.Edges().Any());
        }
        [Test]
        public void CreateWithParamsWithSameVertices()
        {
            var graph = Graph<int>.Create(1, 2, 2, 3, 3, 3, 3, 3, 4, 5, 5, 5, 4);
            HashSet<int> verts = graph.Vertices();
            Assert.That(verts.Count == 5 &&
                        verts.Contains(1) &&
                        verts.Contains(2) &&
                        verts.Contains(3) &&
                        verts.Contains(4) &&
                        verts.Contains(5) &&
                        !graph.Edges().Any());
        }
        [Test]
        public void CreateWithParams()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5);
            HashSet<int> verts = graph.Vertices();
            Assert.That(graph.EdgeCount == 0 &
                        verts.Contains(1) &
                        verts.Contains(2) &
                        verts.Contains(3) &
                        verts.Contains(4) &
                        verts.Contains(5) &
                        !graph.Edges().Any());
        }
        [Test]
        public void AddVertexAddsNew()
        {
            var graph = Graph<int>.Create();
            graph.AddVertex(1);
            HashSet<int> verts = graph.Vertices();
            Assert.That(verts.Count == 1 && verts.Contains(1));
        }
        [Test]
        public void AddVertexThrowsOnSame()
        {
            var graph = Graph<int>.Create(1);
            Assert.Throws<InvalidOperationException>(() => graph.AddVertex(1));
        }
        [Test]
        public void ConnectThrowsOnFirstMissing()
        {
            var graph = Graph<int>.Create(1, 2);
            Assert.Throws<InvalidOperationException>(() => graph.Connect(3, 1, true));
        }
        [Test]
        public void ConnectThrowsOnSecondMissing()
        {
            var graph = Graph<int>.Create(1, 2);
            Assert.Throws<InvalidOperationException>(() => graph.Connect(1, 3, false));
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
            var edges = (HashSet<(int, int, bool)>)graph.Edges();
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
            if (graph.Edges().Any()) Assert.Fail();
            Assert.Pass();
        }
        [Test]
        public void HasVertex()
        {
            var graph = Graph<int>.Create(1);
            Assert.That(graph.HasVertex(1) && !graph.HasVertex(2));
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
        public void NeighboursOf()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, true);
            graph.Connect(1, 4, false);
            HashSet<int> neighboursOf1 = graph.NeighboursOf(1);
            Assert.That(neighboursOf1.Contains(2) &&
                        neighboursOf1.Contains(3) &&
                        neighboursOf1.Contains(4));
        }
        [Test]
        public void Edges()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, true);
            graph.Connect(1, 4, false);
            graph.Connect(2, 1, true);
            var edges = graph.Edges();
            Assert.That(edges.Contains((1, 3, true)) &&
                        edges.Contains((1, 4, false)) &&
                        edges.Contains((2, 1, true)) &&
                        edges.Count == 3);
        }
    }
}
