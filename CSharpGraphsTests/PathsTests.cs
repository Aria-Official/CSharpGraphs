using NUnit.Framework;
using CSharpGraphsLibrary;
namespace CSharpGraphsTests
{
    public class PathsTests
    {
        [Test]
        public void GraphExceptionCheckThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() =>
            { var ls = Paths.Dijkstra.ShortestPathLengths<int>(null!, 0); });
        }
        [Test]
        public void GraphExceptionCheckThrowsOnMissingStart()
        {
            Assert.Throws<InvalidOperationException>(() =>
            { var ps = Paths.Dijkstra.ShortestPaths<double>(Graph<double>.Create(.0), 1.0); });
        }
        [Test]
        public void GraphExceptionCheck2ThrowsOnMissingDestination()
        {
            Assert.Throws<InvalidOperationException>(() =>
            { int l = Paths.Dijkstra.ShortestPathLength<int>(Graph<int>.Create(0), 0, 1); });
        }
        [Test]
        public void WeightedGraphExceptionCheckThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() =>
            { var ls = Paths.Dijkstra.ShortestPathLengths<int, int>(null!, 0,
                (a, b) =>
                {
                    if (a > b) return 1;
                    if (a < b) return -1;
                    return 0;
                },
                (a, b) => a + b);
            });
        }
        [Test]
        public void WeightedGraphExceptionCheckThrowsOnNullComparison()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var graph = WeightedGraph<int, int>.Create(0, 1);
                var pair = Paths.Dijkstra.ShortestPathsAndPathLengths<int, int>(graph, 0,
                    null!, (a, b) => a + b);
            });
        }
        [Test]
        public void WeightedGraphExceptionCheckThrowsOnNullEdgeWeightAddFunction()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var graph = WeightedGraph<int, int>.Create(0, 1);
                var pair = Paths.Dijkstra.ShortestPathsAndPathLengths<int, int>(graph, 0,
                    (a, b) =>
                    {
                        if (a > b) return 1;
                        if (a < b) return -1;
                        return 0;
                    },
                    null!);
            });
        }
        [Test]
        public void WeightedGraphExceptionCheckThrowsOnMissingStart()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var graph = WeightedGraph<int, char>.Create(0);
                var ps = Paths.Dijkstra.ShortestPaths<int, char>(graph, 1,
                    (a, b) =>
                    {
                        if (a > b) return 1;
                        if (a < b) return -1;
                        return 0;
                    },
                    (a, b) => (char)(a + b));
            });
        }
        [Test]
        public void WeightedGraphExceptionCheck2ThrowsOnMissingDestination()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var graph = WeightedGraph<int, int>.Create(0, 1);
                var pair = Paths.AStar.ShortestPathAndPathLength(graph, 0, 1,
                    (a, b) =>
                    {
                        if (a > b) return 1;
                        if (a < b) return -1;
                        return 0;
                    },
                    (a, b) => a + b, null!);
            });
        }
        [Test]
        public void BuildPathThrowsOnNullDict()
        {
            Assert.Throws<ArgumentNullException>(() => { var path = Paths.BuildPath<int>(null!, 0); });
        }
        [Test]
        public void BuildPathThrowsOnMissingVertex()
        {
            Assert.Throws<InvalidOperationException>(() => { var path = Paths.BuildPath<int>(new(), 0); });
        }
    }
}
