using CSharpDataStructuresLibrary;
namespace CSharpGraphsLibrary
{
    public class WeightedGraph<TVertex, TEdgeWeight> where TVertex : notnull
    {
        readonly Dictionary<TVertex, Dictionary<TVertex, AVLTreeIter<TEdgeWeight>>> mapping;
        IComparer<TEdgeWeight> edgeWeightComparer;
        public IComparer<TEdgeWeight> EdgeWeightComparer
        {
            get => edgeWeightComparer;
            set
            {
                if (value is null) throw new ArgumentNullException(
                    $"EdgeWeightComparer.set failed. Specified comparer '{value}' was null.");
                edgeWeightComparer = value;
            }
        }
        public int VertexCount => mapping.Keys.Count;
        public int EdgeCount { get; private set; }
        WeightedGraph(IComparer<TEdgeWeight> comparer)
        {
            mapping = new();
            EdgeCount = 0;
            edgeWeightComparer = comparer;
        }
        WeightedGraph(IComparer<TEdgeWeight> edgeWeightComparer, IEnumerable<TVertex> vertices) : this(edgeWeightComparer)
        {
            foreach (TVertex v in vertices) mapping.Add(v, new());
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create() => new(Comparer<TEdgeWeight>.Default);
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IComparer<TEdgeWeight> edgeWeightComparer)
        {
            if (edgeWeightComparer is null) throw new ArgumentNullException(
                    $"Create() failed. Specified comparer '{edgeWeightComparer}' was null.");
            return new(edgeWeightComparer);
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IEnumerable<TVertex> vertices)
        {
            if (vertices is null) throw new ArgumentNullException(
                    $"Create() failed. Specified enumerable collection of vertices '{vertices}' was null.");
            return new(Comparer<TEdgeWeight>.Default, vertices);
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create(params TVertex[] vertices) =>
            new(Comparer<TEdgeWeight>.Default, vertices);
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IComparer<TEdgeWeight> edgeWeightComparer,
            IEnumerable<TVertex> vertices)
        {
            if (edgeWeightComparer is null) throw new ArgumentNullException(
                $"Create() failed. Specified comparer '{edgeWeightComparer}' was null.");
            if (vertices is null) throw new ArgumentNullException(
                $"Create() failed. Specified enumerable collection of vertices '{vertices}' was null.");
            return new(edgeWeightComparer, vertices);
        }
        public static WeightedGraph<TVertex, TEdgeWeight> Create(IComparer<TEdgeWeight> edgeWeightComparer,
            params TVertex[] vertices)
        {
            if (edgeWeightComparer is null) throw new ArgumentNullException(
                $"Create() failed. Specified comparer '{edgeWeightComparer}' was null.");
            return new(edgeWeightComparer, vertices);
        }
    }
}
