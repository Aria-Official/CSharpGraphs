using CSharpGraphsLibrary;
using GraphEditor.Exceptions;
using GraphEditor.VMs;
using System;
using System.Collections.Generic;
using System.Windows;
namespace GraphEditor.Models
{
    class AlgorithmExecutor
    {
        readonly AlgorithmsVM algorithmsVM;
        public event Action<string>? AlgorithmExecuted;
        public AlgorithmExecutor(AlgorithmsVM algorithmsVM) => this.algorithmsVM = algorithmsVM;
        public void ExecuteDFT()
        {
            try
            {
                InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start start was not specified.",
                                        "Start start doesn't parse to an integer.");
                IEnumerable<int> DFT;
                if (algorithmsVM.Graph is not null)
                    DFT = Traversals.DepthFirstTraversal(algorithmsVM.Graph, start);
                else DFT = Traversals.DepthFirstTraversal(algorithmsVM.WeightedGraph!, start);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.DFT(DFT));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing start"); }
        }
        public void ExecuteBFT()
        {
            try
            {
                InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start start was not specified.",
                                        "Start start doesn't parse to an integer.");
                IEnumerable<int> BFT;
                if (algorithmsVM.Graph is not null)
                    BFT = Traversals.BreadthFirstTraversal(algorithmsVM.Graph, start);
                else BFT = Traversals.BreadthFirstTraversal(algorithmsVM.WeightedGraph!, start);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.BFT(BFT));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing start"); }
        }
        public void ExecuteDijkstraPathLengths()
        {
            try
            {
                InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start start was not specified.",
                                        "Start start doesn't parse to an integer.");
                Dictionary<int, int> pathLengths;
                if (algorithmsVM.Graph is not null)
                    pathLengths = Paths.Dijkstra.ShortestPathLengths(algorithmsVM.Graph, start);
                else pathLengths = Paths.Dijkstra.ShortestPathLengths(algorithmsVM.WeightedGraph!, start,
                    (a, b) =>
                    {
                        if (a > b) return 1;
                        if (a < b) return -1;
                        return 0;
                    },
                    (a, b) => a + b);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.DijkstraPathLengths(pathLengths));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing start"); }
        }
        public void ExecuteDijkstraPaths()
        {
            try
            {
                InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start start was not specified.",
                                        "Start start doesn't parse to an integer.");
                Dictionary<int, int> prevs;
                if (algorithmsVM.Graph is not null)
                    prevs = Paths.Dijkstra.ShortestPaths(algorithmsVM.Graph, start);
                else prevs = Paths.Dijkstra.ShortestPaths(algorithmsVM.WeightedGraph!, start,
                    (a, b) =>
                    {
                        if (a > b) return 1;
                        if (a < b) return -1;
                        return 0;
                    },
                    (a, b) => a + b);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.DijkstraPaths(prevs));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
        public void ExecuteAStarPathLength()
        {
            try
            {
                if (algorithmsVM.WeightedGraph is not null)
                {
                    InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start vertex was not specified.",
                                        "Start vertex doesn't parse to an integer.");
                    InputParser.ParseVertex(algorithmsVM.Destination, out int destination,
                                            "Destination vertex was not specified.",
                                            "Destination vertex doesn't parse to an integer.");
                    AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.AStarPathLength(
                        Paths.AStar.ShortestPathLength(algorithmsVM.WeightedGraph, start, destination,
                            (a, b) =>
                            {
                                if (a > b) return 1;
                                if (a < b) return -1;
                                return 0;
                            },
                            (a, b) => a + b,
                            (_, _) => 0))); // Empty heuristic since no UI to specify it.
                }
                else MessageBox.Show("No weighted graph selected for running A*.",
                                     "No weighted graph");
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
        public void ExecuteAStartPath()
        {
            try
            {
                if (algorithmsVM.WeightedGraph is not null)
                {
                    InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start vertex was not specified.",
                                        "Start vertex doesn't parse to an integer.");
                    InputParser.ParseVertex(algorithmsVM.Destination, out int destination,
                                            "Destination vertex was not specified.",
                                            "Destination vertex doesn't parse to an integer.");
                    AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.AStarPath(
                        Paths.AStar.ShortestPath(algorithmsVM.WeightedGraph, start, destination,
                            (a, b) =>
                            {
                                if (a > b) return 1;
                                if (a < b) return -1;
                                return 0;
                            },
                            (a, b) => a + b,
                            (_, _) => 0))); // Empty heuristic since no UI to specify it.
                }
                else MessageBox.Show("No weighted graph selected for running A*.",
                                     "No weighted graph");
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
        public void ExecuteStrongConnectedComponents()
        {
            try
            {
                List<HashSet<int>> components;
                if (algorithmsVM.Graph is not null)
                    components = Connectivity.StrongConnectedComponents(algorithmsVM.Graph);
                else components = Connectivity.StrongConnectedComponents(algorithmsVM.WeightedGraph!);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.StrongConnectedComponents(components));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Empty graph"); }
        }
        public void ExecuteIsStrongConnected()
        {
            try
            {
                bool isStrongConnected;
                if (algorithmsVM.Graph is not null)
                    isStrongConnected = Connectivity.IsStrongConnected(algorithmsVM.Graph);
                else isStrongConnected = Connectivity.IsStrongConnected(algorithmsVM.WeightedGraph!);
                AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.IsStrongConnected(isStrongConnected));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Empty graph"); }
        }
        public void ExecuteLowestCostWeightTree()
        {
            try
            {
                if (algorithmsVM.WeightedGraph is not null)
                {
                    if (Connectivity.IsStrongConnected(algorithmsVM.WeightedGraph))
                    {
                        InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start vertex was not specified.",
                                        "Start vertex doesn't parse to an integer.");
                        AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.LowestCostWeightTree(
                            WeightTrees.WeightTree(algorithmsVM.WeightedGraph, start,
                                (a, b) =>
                                {
                                    if (a > b) return 1;
                                    if (a < b) return -1;
                                    return 0;
                                })));
                    }
                    else MessageBox.Show("Specified weighted graph wasn't strong connected.",
                                         "Isn't strong connected");
                }
                else MessageBox.Show("No weighted graph selected for running lowest cost weight tree.",
                                     "No weighted graph");
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
        public void ExecuteHighestCostWeightTree()
        {
            try
            {
                if (algorithmsVM.WeightedGraph is not null)
                {
                    if (Connectivity.IsStrongConnected(algorithmsVM.WeightedGraph))
                    {
                        InputParser.ParseVertex(algorithmsVM.Start, out int start,
                                        "Start vertex was not specified.",
                                        "Start vertex doesn't parse to an integer.");
                        AlgorithmExecuted?.Invoke(AlgorithmOutputConstructor.HighestCostWeightTree(
                            WeightTrees.WeightTree(algorithmsVM.WeightedGraph, start,
                                (a, b) =>
                                {
                                    if (a > b) return -1;
                                    if (a < b) return 1;
                                    return 0;
                                })));
                    }
                    else MessageBox.Show("Specified weighted graph wasn't strong connected.",
                                         "Isn't strong connected");
                }
                else MessageBox.Show("No weighted graph selected for running lowest cost weight tree.",
                                     "No weighted graph");
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
    }
}
