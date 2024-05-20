namespace GraphEditor.Models
{
    static class StringConstants
    {
        public const string DepthFirstTraversalsDesc = "Visits all graph vertices starting with the specified start vertex using depth first search (DFS).\nRequires only start vertex input. Destination input will be ignored.\nWorks both for graphs and weighted graphs.";
        public const string BreadthFirstTraversalsDesc = "Visits all graph vertices starting with the specified start vertex using breadth first search (BFS).\nRequires only start vertex input. Destination input will be ignored.\nWorks both for graphs and weighted graphs.";
        public const string DijkstraPathLengthsDesc = "Finds lengths of shortest paths' from the specified start vertex to all other vertices using Dijkstra's algorithm.\nGuarantees to find shortest paths' lengths.\nRequires only start vertex input. Destination input will be ignored.\nWorks both for graphs and weighted graphs.";
        public const string DijkstraPathsDesc = "Finds shortest paths (i.e. sequences of vertices) from the specified start vertex to all other vertices using Dijkstra's algorithm.\nRequires only start vertex input. Destination input will be ignored.\nWorks both for graphs and weighted graphs.";
        public const string AStarPathLengthDesc = "Finds length of shortest path between specified start and destination vertices using A* algorithm with no heuristic.\nDoesn't guarantee to find shortest path length.\nRequires both start and destination vertices input.\nWorks for weighted graphs only.";
        public const string AStarPathDesc = "Finds shortest path (i.e. sequence of vertices) between specified start and destination vertices using A* algorithm with no heuristic.\nRequires both start and destination vertices input.\nWorks for weighted graphs only.";
        public const string StrongConnectedComponentsDesc = "Finds sets of vertices representing strong connected components.\nDoes not require any input.\nWorks both for graphs and weighted graphs.";
        public const string IsStrongConnectedDesc = "Examines whether graph is fully strong connected.\nDoes not require any input.\nWorks both for graphs and weighted graphs.";
        public const string LowestCostWeightTreeDesc = "Finds set of edges representing weight tree of lowest cost.\nWorks only for strong connected weighted graphs (best for disoriented).\nRequires only start vertex input. Destination input will be ignored.";
        public const string HighestCostWeightTreeDesc = "Finds set of edges representing weight tree of highest cost.\nWorks only for strong connected weighted graphs (best for disoriented).\nRequires only start vertex input. Destination input will be ignored.";
    }
}
