using System;

namespace GraphEditor.Models.CustomEventArgs
{
    class VertexEventArgs : EventArgs
    {
        public int Vertex { get; }
        public VertexEventArgs(int vertex) => Vertex = vertex;
    }
}
