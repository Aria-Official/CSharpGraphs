using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class WeightTreesTests
    {
        [Test]
        public void WeightTreeThrowsOnNullGraph()
        {
            Assert.Throws<ArgumentNullException>(() =>
            { var weightTree = WeightTrees.WeightTree<int, int>(null!, 0,
                (a, b) =>
                {
                    if (a > b) return 1;
                    if (a < b) return -1;
                    return 0;
                });
            });
        }
        [Test]
        public void WeightTreeThrowOnMissingStart()
        {
            Assert.Throws<InvalidOperationException>(() =>
            { var weightTree = WeightTrees.WeightTree<int, int>(WeightedGraph<int, int>.Create(), 0,
                (a, b) =>
                {
                    if (a > b) return 1;
                    if (a < b) return -1;
                    return 0;
                });
            });
        }
        [Test]
        public void WeightTreeThrowsOnNullEdgeWeightComparisonFunction()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var weightTree = WeightTrees.WeightTree<int, int>(WeightedGraph<int, int>.Create(0), 0,
                null!);
            });
        }
        [Test]
        public void WeightTreeFindsMinWeightTree()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5, 6, 7);
            graph.Connect(1, 2, false, 7);
            graph.Connect(1, 4, false, 5);
            graph.Connect(2, 3, false, 8);
            graph.Connect(2, 4, false, 9);
            graph.Connect(2, 5, false, 7);
            graph.Connect(3, 5, false, 5);
            graph.Connect(4, 5, false, 15);
            graph.Connect(4, 6, false, 6);
            graph.Connect(5, 6, false, 8);
            graph.Connect(5, 7, false, 9);
            graph.Connect(6, 7, false, 11);
            var weightTree = WeightTrees.WeightTree(graph, 4, (a, b) =>
            {
                if (a > b) return 1;
                if (a < b) return -1;
                return 0;
            });
            Assert.That(weightTree.Count == 6 &&
                        weightTree.Contains((4, 1, 5)) &&
                        weightTree.Contains((1, 2, 7)) &&
                        weightTree.Contains((2, 5, 7)) &&
                        weightTree.Contains((5, 3, 5)) &&
                        weightTree.Contains((4, 6, 6)) &&
                        weightTree.Contains((5, 7, 9)));
        }
        [Test]
        public void WeightTreeFindsMaxWeightTree()
        {
            var graph = WeightedGraph<int, int>.Create(1, 2, 3, 4, 5, 6, 7);
            graph.Connect(1, 2, false, 15);
            graph.Connect(1, 4, false, 20);
            graph.Connect(2, 3, false, 8);
            graph.Connect(2, 4, false, 9);
            graph.Connect(2, 5, false, 14);
            graph.Connect(3, 5, false, 12);
            graph.Connect(4, 5, false, 15);
            graph.Connect(4, 6, false, 16);
            graph.Connect(5, 6, false, 8);
            graph.Connect(5, 7, false, 13);
            graph.Connect(6, 7, false, 11);
            var weightTree = WeightTrees.WeightTree(graph, 4, (a, b) =>
            {
                if (a > b) return -1;
                if (a < b) return 1;
                return 0;
            });
            Assert.That(weightTree.Count == 6 &&
                        weightTree.Contains((4, 1, 20)) &&
                        weightTree.Contains((1, 2, 15)) &&
                        weightTree.Contains((4, 5, 15)) &&
                        weightTree.Contains((5, 3, 12)) &&
                        weightTree.Contains((4, 6, 16)) &&
                        weightTree.Contains((5, 7, 13)));
        }
    }
}
