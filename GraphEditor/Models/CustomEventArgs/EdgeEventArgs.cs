using System;
namespace GraphEditor.Models.CustomEventArgs
{
    class EdgeEventArgs : EventArgs
    {
        public int EdgeStart { get; }
        public int EdgeEnd { get; }
        public EdgeEventArgs(int edgeStart, int edgeEnd)
        {
            EdgeStart = edgeStart;
            EdgeEnd = edgeEnd;
        }
    }
}
