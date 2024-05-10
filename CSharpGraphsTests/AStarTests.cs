using NUnit.Framework;
using CSharpGraphsLibrary;
using System.Drawing;
namespace CSharpGraphsTests
{
    [TestFixture]
    public class AStarTests
    {
        [Test]
        public void ShortestPathLengthWhenReachable()
        {
            Point p0 = new(18, 18),
                  p1 = new(2, 15),
                  p2 = new(13, 4),
                  p3 = new(8, 30),
                  p4 = new(29, 4),
                  p5 = new(38, 14),
                  p6 = new(28, 27),
                  p7 = new(37, 32);
            var graph = WeightedGraph<Point, int>.Create(p0, p1, p2, p3, p4, p5, p6, p7);
            graph.Connect(p0, p1, false, 5);
            graph.Connect(p0, p2, false, 4);
            graph.Connect(p1, p3, false, 8);
            graph.Connect(p2, p4, true, 7);
            graph.Connect(p3, p7, true, 22);
            graph.Connect(p4, p5, true, 8);
            graph.Connect(p5, p7, false, 16);
            graph.Connect(p0, p6, true, 10);
            graph.Connect(p6, p7, false, 11);
            int l = Paths.AStar.ShortestPathLength(graph, p0, p7,
                Comparer<int>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            Assert.That(l == 21);
        }
        [Test]
        public void ShortestPathLengthWhenNotReachable()
        {
            Point p0 = new(0, 0),
                  p1 = new(1, 1);
            var graph = WeightedGraph<Point, double>.Create(p0, p1);
            double l = Paths.AStar.ShortestPathLength(graph, p0, p1,
                Comparer<double>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            Assert.That(l == 0);
        }
        [Test]
        public void ShortestPathLengthWhenStartIsDestination()
        {
            Point p0 = new(0, 0);
            var graph = WeightedGraph<Point, int>.Create(p0);
            int l = Paths.AStar.ShortestPathLength(graph, p0, p0,
                Comparer<int>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            Assert.That(l == 0);
        }
        [Test]
        public void ShortestPathWhenReachable()
        {
            Point p0 = new(18, 18),
                  p1 = new(2, 15),
                  p2 = new(13, 4),
                  p3 = new(8, 30),
                  p4 = new(29, 4),
                  p5 = new(38, 14),
                  p6 = new(28, 27),
                  p7 = new(37, 32);
            var graph = WeightedGraph<Point, int>.Create(p0, p1, p2, p3, p4, p5, p6, p7);
            graph.Connect(p0, p1, false, 5);
            graph.Connect(p0, p2, false, 4);
            graph.Connect(p1, p3, false, 8);
            graph.Connect(p2, p4, true, 7);
            graph.Connect(p3, p7, true, 22);
            graph.Connect(p4, p5, true, 8);
            graph.Connect(p5, p7, false, 16);
            graph.Connect(p0, p6, true, 10);
            graph.Connect(p6, p7, false, 11);
            List<Point> p = Paths.AStar.ShortestPath(graph, p0, p7,
                Comparer<int>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)))!;
            Assert.That(p.Count == 3 &&
                        p[0] == p0 &&
                        p[1] == p6 &&
                        p[2] == p7);
        }
        [Test]
        public void ShortestPathWhenNotReachable()
        {
            Point p0 = new(0, 0),
                  p1 = new(1, 1);
            var graph = WeightedGraph<Point, double>.Create(p0, p1);
            List<Point>? p = Paths.AStar.ShortestPath(graph, p0, p1,
                Comparer<double>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            Assert.That(p is null);
        }
        [Test]
        public void ShortestPathWhenStartIsDestination()
        {
            Point p0 = new(0, 0);
            var graph = WeightedGraph<Point, double>.Create(p0);
            List<Point>? p = Paths.AStar.ShortestPath(graph, p0, p0,
                Comparer<double>.Default, (a, b) => a + b,
                (p1, p2) => (int)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            Assert.That(p is null);
        }
    }
}
