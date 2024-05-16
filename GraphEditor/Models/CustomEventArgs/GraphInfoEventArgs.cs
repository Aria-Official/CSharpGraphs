using System;

namespace GraphEditor.Models.CustomEventArgs
{
    class GraphInfoEventArgs : EventArgs
    {
        public GraphInfo GraphInfo { get; }
        public GraphInfoEventArgs(GraphInfo graphInfo)
        {
            GraphInfo = graphInfo;
        }
    }
}
