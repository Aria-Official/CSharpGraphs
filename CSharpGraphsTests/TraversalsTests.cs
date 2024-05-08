using CSharpGraphsLibrary;
using NUnit.Framework;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class TraversalsTests
    {
        [Test]
        public void DFT1()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5, 6, 7, 8);
            graph.Connect(1, 2, true);
            graph.Connect(2, 3, true);
            graph.Connect(1, 4, false);
            graph.Connect(2, 4, false);
            graph.Connect(4, 3, true);
            graph.Connect(1, 2, true);
            graph.Connect(5, 6, true);
            graph.Connect(6, 7, true);
            graph.Connect(7, 5, false);
            IEnumerable<int> dft = Traversals.DepthFirstTraversal(graph, 1);
            Assert.That(string.Join(string.Empty, dft) == "12345678");
        }
        [Test]
        public void DFT2()
        {
            var graph = Graph<int>.Create(0, 1, 2, 4, 5, 6, 7, 8, 9);
            graph.Connect(1, 6, true);
            graph.Connect(6, 4, true);
            graph.Connect(4, 1, false);
            graph.Connect(6, 5, true);
            graph.Connect(4, 5, false);
            graph.Connect(5, 2, true);
            graph.Connect(7, 8, false);
            graph.Connect(8, 9, true);
            graph.Connect(7, 0, true);
            graph.Connect(9, 0, false);
            IEnumerable<int> dft = Traversals.DepthFirstTraversal(graph, 1);
            Assert.That(string.Join(string.Empty, dft) == "164520978");
        }
        [Test]
        public void DFT3()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            IEnumerable<int> dft = Traversals.DepthFirstTraversal(graph, 1);
            Assert.That(string.Join(string.Empty, dft) == "1023456789");
        }
        [Test]
        public void BFT1()
        {
            var graph = Graph<int>.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 0);
            graph.Connect(1, 2, true);
            graph.Connect(1, 3, false);
            graph.Connect(2, 4, true);
            graph.Connect(2, 5, false);
            graph.Connect(5, 6, true);
            graph.Connect(3, 7, true);
            graph.Connect(3, 8, false);
            graph.Connect(9, 0, true);
            IEnumerable<int> dft = Traversals.BreadthFirstTraversal(graph, 1);
            Assert.That(string.Join(string.Empty, dft) == "1234567890");
        }
        [Test]
        public void BFT2()
        {
            var graph = Graph<int>.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            IEnumerable<int> dft = Traversals.BreadthFirstTraversal(graph, 1);
            Assert.That(string.Join(string.Empty, dft) == "1023456789");
        }
    }
}
