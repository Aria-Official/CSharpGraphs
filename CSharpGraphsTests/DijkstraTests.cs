using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class DijkstraTests
    {
        [Test]
        public void GraphExceptionsCheckThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() => { var ls = Dijkstra.ShortestPathLengths<int>(null!, 0); });
        }
        [Test]
        public void GraphExceptionsCheckThrowsOnEmptyGraph()
        {
            Assert.Throws<InvalidOperationException>(() => { var pair = Dijkstra.ShortestPathsAndPathLengths<char>(Graph<char>.Create(), 'A'); });
        }
        [Test]
        public void GraphExceptionsCheckThrowsOnMissingStart()
        {
            Assert.Throws<InvalidOperationException>(() => { var ps = Dijkstra.ShortestPaths<double>(Graph<double>.Create(.0), 1.0); });
        }
        [Test]
        public void GraphExceptionsCheck2ThrowsOnMissingDestination()
        {
            Assert.Throws<InvalidOperationException>(() => { int l = Dijkstra.ShortestPathLength<int>(Graph<int>.Create(0), 0, 1); });
        }
        [Test]
        public void ShortestPathLengthsForGraph()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, false);
            graph.Connect(2, 4, true);
            graph.Connect(2, 6, true);
            graph.Connect(4, 6, true);
            graph.Connect(6, 7, false);
            graph.Connect(7, 8, false);
            graph.Connect(3, 5, true);
            graph.Connect(5, 9, true);
            graph.Connect(9, 8, true);
            Dictionary<int, int> ls = Dijkstra.ShortestPathLengths<int>(graph, 1);
            Assert.That(ls.Count == 9 &&
                        ls[1] == 0 &&
                        ls[2] == 1 &&
                        ls[3] == 1 &&
                        ls[4] == 2 &&
                        ls[5] == 2 &&
                        ls[6] == 2 &&
                        ls[7] == 3 &&
                        ls[8] == 4 &&
                        ls[9] == 3);
        }
        [Test]
        public void ShortestPathsAndPathLengthsForGraph()
        {
            var graph = Graph<char>.Create('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M');
            graph.Connect('A', 'K', true);
            graph.Connect('E', 'K', true);
            graph.Connect('D', 'E', true);
            graph.Connect('D', 'G', true);
            graph.Connect('G', 'B', true);
            graph.Connect('D', 'C', false);
            graph.Connect('E', 'C', true);
            graph.Connect('C', 'G', false);
            graph.Connect('H', 'G', true);
            graph.Connect('I', 'H', true);
            graph.Connect('C', 'F', true);
            graph.Connect('F', 'I', false);
            graph.Connect('I', 'J', true);
            graph.Connect('L', 'J', false);
            (Dictionary<char, int> ls, Dictionary<char, char> ps) = Dijkstra.ShortestPathsAndPathLengths<char>(graph, 'G');
            bool lsCorrect = ls.Count == 11 &&
                ls['B'] == 1 &&
                ls['C'] == 1 &&
                ls['D'] == 2 &&
                ls['E'] == 3 &&
                ls['F'] == 2 &&
                ls['G'] == 0 &&
                ls['H'] == 4 &&
                ls['I'] == 3 &&
                ls['J'] == 4 && 
                ls['K'] == 4 &&
                ls['L'] == 5,
            psCorrect = ps.Count == 10 &&
                ps['B'] == 'G' &&
                ps['C'] == 'G' &&
                ps['D'] == 'C' &&
                ps['E'] == 'D' &&
                ps['F'] == 'C' &&
                ps['H'] == 'I' &&
                ps['I'] == 'F' &&
                ps['J'] == 'I' &&
                ps['K'] == 'E' &&
                ps['L'] == 'J' &&
                !ps.ContainsKey('A') &&
                !ps.ContainsKey('M');
            Assert.That(lsCorrect && psCorrect);
        }
        [Test]
        public void ShortestPathsForGraph()
        {
            var graph = Graph<byte>.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            graph.Connect(1, 2, true);
            graph.Connect(2, 3, true);
            graph.Connect(3, 4, true);
            graph.Connect(4, 5, false);
            graph.Connect(5, 10, false);
            graph.Connect(5, 6, true);
            graph.Connect(2, 9, true);
            graph.Connect(11, 3, true);
            graph.Connect(1, 6, true);
            graph.Connect(6, 7, true);
            graph.Connect(8, 7, false);
            Dictionary<byte, byte> ps = Dijkstra.ShortestPaths<byte>(graph, 1);
            Assert.That(ps.Count == 9 &&
                        ps[2] == 1 &&
                        ps[3] == 2 &&
                        ps[4] == 3 &&
                        ps[5] == 4 &&
                        ps[6] == 1 &&
                        ps[7] == 6 &&
                        ps[8] == 7 &&
                        ps[9] == 2 &&
                        ps[10] == 5);
        }
        [Test]
        public void ShortestPathLengthForGraphWhenReachable()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            graph.Connect(1, 2, false);
            graph.Connect(1, 3, false);
            graph.Connect(1, 6, false);
            graph.Connect(2, 5, false);
            graph.Connect(3, 4, false);
            graph.Connect(5, 7, false);
            graph.Connect(6, 8, false);
            graph.Connect(4, 0, false);
            graph.Connect(7, 0, false);
            graph.Connect(8, 0, false);
            graph.Connect(9, 0, false);
            Assert.That(Dijkstra.ShortestPathLength(graph, 1, 0) == 3);
        }
        [Test]
        public void ShortestPathLengthForGraphWhenNotReachable()
        {
            var graph = Graph<int>.Create(0, 1);
            Assert.That(Dijkstra.ShortestPathLength(graph, 0, 1) == -1);
        }
        [Test]
        public void ShortestPathLengthForGraphWhenStartIsDestination()
        {
            var graph = Graph<int>.Create(0);
            Assert.That(Dijkstra.ShortestPathLength(graph, 0, 0) == 0);
        }
        [Test]
        public void ShortestPathAndPathLengthForGraphWhenReachable()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            graph.Connect(1, 2, false);
            graph.Connect(1, 3, false);
            graph.Connect(1, 6, false);
            graph.Connect(2, 5, false);
            graph.Connect(3, 4, false);
            graph.Connect(5, 7, false);
            graph.Connect(6, 8, false);
            graph.Connect(4, 0, false);
            graph.Connect(7, 0, false);
            graph.Connect(8, 0, false);
            graph.Connect(9, 0, false);
            var pair = Dijkstra.ShortestPathAndPathLength(graph, 1, 0);
            List<int> path = pair.Path!;
            Assert.That(pair.PathLength == 3 &&
                        path.Count == 4 &&
                        path[0] == 1 &&
                        path[1] == 3 &&
                        path[2] == 4 &&
                        path[3] == 0);
        }
        [Test]
        public void ShortestPathAndPathLengthForGraphWhenNotReachable()
        {
            var graph = Graph<int>.Create(0, 1);
            var pair = Dijkstra.ShortestPathAndPathLength(graph, 0, 1);
            Assert.That(pair.Path is null && pair.PathLength == -1);
        }
        [Test]
        public void ShortestPathAndPathLengthForGraphWhenStartIsDestination()
        {
            var graph = Graph<int>.Create(0);
            var pair = Dijkstra.ShortestPathAndPathLength(graph, 0, 0);
            Assert.That(pair.Path is null && pair.PathLength == 0);
        }
        [Test]
        public void ShortestPathForGraphWhenReachable()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            graph.Connect(1, 2, false);
            graph.Connect(1, 3, false);
            graph.Connect(1, 6, false);
            graph.Connect(2, 5, false);
            graph.Connect(3, 4, false);
            graph.Connect(5, 7, false);
            graph.Connect(6, 8, false);
            graph.Connect(4, 0, false);
            graph.Connect(7, 0, false);
            graph.Connect(8, 0, false);
            graph.Connect(9, 0, false);
            List<int> path = Dijkstra.ShortestPath(graph, 1, 0)!;
            Assert.That(path is not null && path.Count == 4 &&
                        path[0] == 1 &&
                        path[1] == 3 &&
                        path[2] == 4 &&
                        path[3] == 0);
        }
        [Test]
        public void ShortestPathForGraphWhenNotReachable()
        {
            var graph = Graph<int>.Create(0, 1);
            Assert.That(Dijkstra.ShortestPath(graph, 0, 1) is null);
        }
        [Test]
        public void ShortestPathForGraphWhenStartIsDestination()
        {
            var graph = Graph<int>.Create(0);
            Assert.That(Dijkstra.ShortestPath(graph, 0, 0) is null);
        }
        [Test]
        public void WeightedGraphExceptionsCheckThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() =>
            { var ls = Dijkstra.ShortestPathLengths<int, int>(null!, 0, Comparer<int>.Default, (a, b) => a + b); });
        }
        [Test]
        public void WeightedGraphExceptionsCheckThrowsOnEmptyGraph()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var graph = WeightedGraph<int, double>.Create();
                var pair = Dijkstra.ShortestPathsAndPathLengths<int, double>(graph, 0, Comparer<double>.Default, (a, b) => a + b);
            });
        }
        [Test]
        public void WeightedGraphExceptionsCheckThrowsOnMissingStart()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var graph = WeightedGraph<int, char>.Create('A');
                var ps = Dijkstra.ShortestPaths<int, char>(graph, 'B', Comparer<char>.Default, (a, b) => (char)(a + b));
            });
        }
        [Test]
        public void ShortestPathLengthsForWeightedGraph()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5, 6, 7, 8, 9);
            graph.Connect(1, 2, false, 1);
            graph.Connect(2, 7, true, 2);
            graph.Connect(2, 3, false, 3);
            graph.Connect(3, 4, false, 4);
            graph.Connect(1, 4, false, 9);
            graph.Connect(8, 4, true, 6);
            graph.Connect(4, 6, false, 5);
            graph.Connect(1, 2, false, 1);
            graph.Connect(6, 5, false, 7);
            graph.Connect(1, 5, true, 8);
            Dictionary<int, int> ls = Dijkstra.ShortestPathLengths(graph, 1, Comparer<int>.Default, (a, b) => a + b);
            Assert.That(ls.Count == 7 &&
                        ls[1] == 0 &&
                        ls[2] == 1 &&
                        ls[3] == 4 &&
                        ls[4] == 8 &&
                        ls[5] == 8 &&
                        ls[6] == 13 &&
                        ls[7] == 3);
        }
        [Test]
        public void ShortestPathsAndPathLengthsForWeightedGraph()
        {
            var graph = WeightedGraph<int, int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            graph.Connect(1, 2, true, 10);
            graph.Connect(1, 4, true, 13);
            graph.Connect(2, 3, true, 1);
            graph.Connect(3, 4, false, 1);
            graph.Connect(4, 5, true, 4);
            graph.Connect(4, 6, true, 3);
            graph.Connect(3, 6, true, 2);
            graph.Connect(6, 5, true, 8);
            graph.Connect(2, 7, true, 2);
            graph.Connect(6, 7, true, 4);
            graph.Connect(6, 9, false, 6);
            graph.Connect(7, 8, true, 12);
            graph.Connect(8, 9, true, 20);
            graph.Connect(10, 4, true, 9);
            (Dictionary<int, int> ls, Dictionary<int, int> ps) = Dijkstra.ShortestPathsAndPathLengths(graph, 1,
                Comparer<int>.Default, (a, b) => a + b);
            bool lsCorrect = ls.Count == 9 &&
                 ls[1] == 0 &&
                 ls[2] == 10 &&
                 ls[3] == 11 &&
                 ls[4] == 12 &&
                 ls[5] == 16 &&
                 ls[6] == 13 &&
                 ls[7] == 12 &&
                 ls[8] == 24 &&
                 ls[9] == 19,
                 psCorrect = ps.Count == 8 &&
                 ps[2] == 1 &&
                 ps[3] == 2 &&
                 ps[4] == 3 &&
                 ps[5] == 4 &&
                 ps[6] == 3 &&
                 ps[7] == 2 &&
                 ps[8] == 7 &&
                 ps[9] == 6;
            Assert.That(lsCorrect && psCorrect);
        }
        [Test]
        public void ShortestPathsForWeightedGraph()
        {
            var graph = WeightedGraph<int, decimal>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            graph.Connect(1, 2, true, 2.0m);
            graph.Connect(1, 3, true, 3.0m);
            graph.Connect(1, 7, false, 10.0m);
            graph.Connect(2, 9, true, 6.0m);
            graph.Connect(2, 8, true, 7.0m);
            graph.Connect(2, 4, true, 3.0m);
            graph.Connect(4, 5, false, 7.0m);
            graph.Connect(3, 5, true, 15.0m);
            graph.Connect(10, 3, true, 1.0m);
            graph.Connect(5, 6, true, 2.0m);
            graph.Connect(7, 6, false, 1.0m);
            Dictionary<int, int> ps = Dijkstra.ShortestPaths(graph, 1, Comparer<decimal>.Default, (a, b) => a + b);
            Assert.That(ps.Count == 8 &&
                        ps[2] == 1 &&
                        ps[3] == 1 &&
                        ps[4] == 2 &&
                        ps[5] == 4 &&
                        ps[6] == 7 &&
                        ps[7] == 1 &&
                        ps[8] == 2 &&
                        ps[9] == 2);
        }
        [Test]
        public void BuildPathThrowsOnNullDict()
        {
            Assert.Throws<ArgumentNullException>(() => { var path = Dijkstra.BuildPath<int>(null!, 0); });
        }
        [Test]
        public void BuildPathThrowsOnMissingVertex()
        {
            Assert.Throws<InvalidOperationException>(() => { var path = Dijkstra.BuildPath<int>(new(), 0); });
        }
    }
}
