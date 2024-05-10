using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class ConnectivityTests
    {
        [Test]
        public void StrongConnectedComponentsThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() => 
            { var cs = Connectivity.StrongConnectedComponents<int>(null!); });
        }
        [Test]
        public void StrongConnectedComponentsEmptyGraph()
        {
            Assert.Throws<InvalidOperationException>(() =>
            { var cs = Connectivity.StrongConnectedComponents<char>(WeightedGraph<char, int>.Create()); });
        }
        [Test]
        public void StrongConnectedComponents1()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5, 6, 7, 8);
            graph.Connect(1, 2, true);
            graph.Connect(2, 3, true);
            graph.Connect(3, 4, false);
            graph.Connect(5, 1, true);
            graph.Connect(2, 5, true);
            graph.Connect(3, 7, true);
            graph.Connect(4, 8, false);
            graph.Connect(5, 6, true);
            graph.Connect(6, 7, false);
            graph.Connect(8, 7, true);
            var cs = Connectivity.StrongConnectedComponents(graph);
            HashSet<int> c0 = cs[0], c1 = cs[1], c2 = cs[2];
            bool correct0 = c0.Count == 2 && c0.Contains(6) && c0.Contains(7),
                 correct1 = c1.Count == 3 && c1.Contains(3) && c1.Contains(4) && c1.Contains(8),
                 correct2 = c2.Count == 3 && c2.Contains(1) && c2.Contains(2) && c2.Contains(5);
            Assert.That(correct0 && correct1 && correct2);
        }
        [Test]
        public void StrongConnectedComponents2()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5, 6, 7, 8);
            graph.Connect(1, 2, true, 0);
            graph.Connect(2, 3, true, 0);
            graph.Connect(3, 1, true, 0);
            graph.Connect(4, 2, true, 0);
            graph.Connect(4, 3, true, 0);
            graph.Connect(4, 5, false, 0);
            graph.Connect(5, 6, true, 0);
            graph.Connect(1, 2, true, 0);
            graph.Connect(6, 3, true, 0);
            graph.Connect(6, 7, false, 0);
            graph.Connect(8, 7, true, 0);
            graph.Connect(8, 5, true, 0);
            var cs = Connectivity.StrongConnectedComponents(graph);
            HashSet<int> c0 = cs[0], c1 = cs[1], c2 = cs[2], c3 = cs[3];
            bool correct0 = c0.Count == 3 && c0.Contains(1) && c0.Contains(2) && c0.Contains(3),
                 correct1 = c1.Count == 2 && c1.Contains(6) && c1.Contains(7),
                 correct2 = c2.Count == 2 && c2.Contains(4) && c2.Contains(5),
                 correct3 = c3.Count == 1 && c3.Contains(8);
            Assert.That(correct0 && correct1 && correct2 && correct3);
        }
        [Test]
        public void StrongConnectedComponents3()
        {
            var graph = Graph<char>.Create('A', 'B', 'C', 'D');
            graph.Connect('A', 'B', true);
            graph.Connect('B', 'C', false);
            graph.Connect('C', 'D', true);
            graph.Connect('A', 'D', false);
            var cs = Connectivity.StrongConnectedComponents(graph);
            HashSet<char> c0 = cs[0];
            bool all = c0.Contains('A') && c0.Contains('B') && c0.Contains('C') && c0.Contains('D');
            Assert.That(cs.Count == 1 && all);
        }
        [Test]
        public void IsStrongConnected1()
        {
            var graph = WeightedGraph<int, int>.Create(0);
            Assert.That(Connectivity.IsStrongConnected(graph));
        }
        [Test]
        public void IsStrongConnected2()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4);
            graph.Connect(1, 2, true);
            graph.Connect(2, 3, true);
            graph.Connect(3, 4, true);
            graph.Connect(4, 1, true);
            Assert.That(Connectivity.IsStrongConnected(graph));
        }
        [Test]
        public void CreateSubgraphsFromStrongConnectedComponentsForGraph()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5, 6, 7, 8);
            graph.Connect(1, 2, true);
            graph.Connect(2, 3, true);
            graph.Connect(3, 4, false);
            graph.Connect(5, 1, true);
            graph.Connect(2, 5, true);
            graph.Connect(3, 7, true);
            graph.Connect(4, 8, false);
            graph.Connect(5, 6, true);
            graph.Connect(6, 7, false);
            graph.Connect(8, 7, true);
            var subgraphs = Connectivity.CreateSubgraphsFromStrongConnectedComponents(graph);
            Graph<int> sg0 = subgraphs[0], sg1 = subgraphs[1], sg2 = subgraphs[2];
            bool correct0 = sg0.VertexCount == 2 &&
                            sg0.EdgeCount == 1 &&
                            sg0.HasVertex(6) &&
                            sg0.HasVertex(7) &&
                            sg0.HasEdge(6, 7, false),
                 correct1 = sg1.VertexCount == 3 &&
                            sg1.EdgeCount == 2 &&
                            sg1.HasVertex(3) &&
                            sg1.HasVertex(4) &&
                            sg1.HasVertex(8) &&
                            sg1.HasEdge(3, 4, false) &&
                            sg1.HasEdge(4, 8, false),
                 correct2 = sg2.VertexCount == 3 &&
                            sg2.EdgeCount == 3 &&
                            sg2.HasVertex(1) &&
                            sg2.HasVertex(2) &&
                            sg2.HasVertex(5) &&
                            sg2.HasEdge(1, 2, true) &&
                            sg2.HasEdge(2, 5, true) &&
                            sg2.HasEdge(5, 1, true);
            Assert.That(correct0 && correct1 && correct2);
        }
        [Test]
        public void CreateSubgraphsFromStrongConnectedComponentsForWeightedGraph()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5, 6, 7, 8);
            graph.Connect(1, 2, true, 0);
            graph.Connect(2, 3, true, 0);
            graph.Connect(3, 1, true, 0);
            graph.Connect(4, 2, true, 0);
            graph.Connect(4, 3, true, 0);
            graph.Connect(4, 5, false, 0);
            graph.Connect(5, 6, true, 0);
            graph.Connect(1, 2, true, 0);
            graph.Connect(6, 3, true, 0);
            graph.Connect(6, 7, false, 0);
            graph.Connect(8, 7, true, 0);
            graph.Connect(8, 5, true, 0);
            var subgraphs = Connectivity.CreateSubgraphsFromStrongConnectedComponents(graph);
            WeightedGraph<int, int> sg0 = subgraphs[0], sg1 = subgraphs[1],
                                    sg2 = subgraphs[2], sg3 = subgraphs[3];
            bool correct0 = sg0.VertexCount == 3 &&
                            sg0.EdgeCount == 3 &&
                            sg0.HasVertex(1) &&
                            sg0.HasVertex(2) &&
                            sg0.HasVertex(3) &&
                            sg0.HasEdge(1, 2, true) &&
                            sg0.HasEdge(2, 3, true) &&
                            sg0.HasEdge(3, 1, true),
                 correct1 = sg1.VertexCount == 2 &&
                            sg1.EdgeCount == 1 &&
                            sg1.HasVertex(6) &&
                            sg1.HasVertex(7) &&
                            sg1.HasEdge(6, 7, false),
                 correct2 = sg2.VertexCount == 2 &&
                            sg2.EdgeCount == 1 &&
                            sg2.HasVertex(4) &&
                            sg2.HasVertex(5) &&
                            sg2.HasEdge(5, 4, false),
                 correct3 = sg3.VertexCount == 1 &&
                            sg3.EdgeCount == 0 &&
                            sg3.HasVertex(8);
            Assert.That(correct0 && correct1 && correct2 && correct3);
        }
    }
}
