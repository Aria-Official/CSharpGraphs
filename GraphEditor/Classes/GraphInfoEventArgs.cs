using System;
namespace GraphEditor.Classes
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
