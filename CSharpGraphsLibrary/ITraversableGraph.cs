namespace CSharpGraphsLibrary
{
    public interface ITraversableGraph<T>
    {
        int VertexCount { get; }
        bool HasVertex(T vertex);
        IEnumerable<T>? NeighboursOf(T vertex);
        IEnumerable<T>? Vertices();
    }
}
