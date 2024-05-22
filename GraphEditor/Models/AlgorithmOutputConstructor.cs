using CSharpGraphsLibrary;
using System.Collections.Generic;
using System.Text;
namespace GraphEditor.Models
{
    static class AlgorithmOutputConstructor
    {
        static readonly StringBuilder Builder = new();
        public static string DFT(IEnumerable<int> e)
        {
            Builder.AppendLine("Depth first traversal executed.");
            Builder.AppendLine("Result is the following vertex sequence:");
            Builder.AppendLine(string.Join(' ', e) + ".");
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string BFT(IEnumerable<int> e)
        {
            Builder.AppendLine("Breath first traversal executed.");
            Builder.AppendLine("Result is the following vertex sequence:");
            Builder.AppendLine(string.Join(' ', e) + ".");
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string DijkstraPathLengths(Dictionary<int, int> pathLengths)
        {
            Builder.AppendLine("Dijkstra executed.");
            Builder.AppendLine("Result is the following (vertex : pathLength) mapping.");
            Builder.AppendLine("If some vertex is not in the mapping then it's unreachable from the start vertex.");
            foreach ((int v, int pathLength) in pathLengths)
                Builder.AppendLine($"{v} : {pathLength},");
            Builder.Length -= 3; // Removes CRLF and last comma.
            Builder.Append('.');
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string DijkstraPaths(Dictionary<int, int> prevs)
        {
            Builder.AppendLine("Dijkstra executed.");
            Builder.AppendLine("Result is the following (vertex : path) mapping.");
            Builder.AppendLine("If some vertex is not in the mapping then it's unreachable from the start vertex.");
            foreach (int v in prevs.Keys)
                Builder.AppendLine($"{v} : {string.Join(' ', Paths.BuildPath(prevs, v))},");
            Builder.Length -= 3; // Removes CRLF and last comma.
            Builder.Append('.');
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string AStarPathLength(int pathLength)
        {
            Builder.AppendLine("A* executed.");
            Builder.AppendLine($"Result is the following path length: {pathLength}.");
            Builder.AppendLine("If path length is '0' for non-start destination vertex then it's unreachable from the start vertex.");
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string AStarPath(List<int>? path)
        {
            Builder.AppendLine("A* executed.");
            if (path is not null)
                 Builder.AppendLine($"Result is the following path: {string.Join(' ', path)}.");
            else Builder.AppendLine("Destination vertex was unreachable from the start vertex.");
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string StrongConnectedComponents(List<HashSet<int>> components)
        {
            Builder.AppendLine("Strong connected components executed.");
            Builder.AppendLine("Result are the following components:");
            for (int i = 0; i < components.Count; ++i)
                Builder.AppendLine($"component {i + 1}: {string.Join(' ', components[i])};");
            Builder.Length -= 3; // Removes CRLF and last semicolon.
            Builder.Append('.');
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string IsStrongConnected(bool value)
        {
            Builder.AppendLine("Is strong connected executed.");
            Builder.AppendLine($"Result is the following boolean value: {value.ToString().ToLower()}.");
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string LowestCostWeightTree(List<(int v1, int v2, int weight)> edges)
        {
            Builder.AppendLine("Lowest cost weight tree executed.");
            Builder.AppendLine("Result is the following list of weighted edges:");
            foreach ((int v1, int v2, int weight) in edges)
                Builder.AppendLine($"({v1}, {v2}; {weight});");
            Builder.Length -= 3; // Removes CRLF and last semicolon.
            Builder.Append('.');
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
        public static string HighestCostWeightTree(List<(int v1, int v2, int weight)> edges)
        {
            Builder.AppendLine("Highest cost weight tree executed.");
            Builder.AppendLine("Result is the following list of weighted edges:");
            foreach ((int v1, int v2, int weight) in edges)
                Builder.AppendLine($"({v1}, {v2}; {weight});");
            Builder.Length -= 3; // Removes CRLF and last semicolon.
            Builder.Append('.');
            string result = Builder.ToString();
            Builder.Clear();
            return result;
        }
    }
}
