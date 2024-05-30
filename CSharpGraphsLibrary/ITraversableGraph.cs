namespace CSharpGraphsLibrary
{
    /// <summary>
    /// Represents set of operations that are used to traverse graph.
    /// </summary>
    /// <typeparam name="T">Type of graph vertex.</typeparam>
    public interface ITraversableGraph<T>
    {
        int VertexCount { get; }
        bool HasVertex(T vertex);
        IEnumerable<T>? NeighboursOf(T vertex);
        IEnumerable<T>? Vertices();
    }
}
