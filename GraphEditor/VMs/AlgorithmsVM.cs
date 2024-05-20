using CSharpGraphsLibrary;
using GraphEditor.Commands;
using GraphEditor.Models;
using GraphEditor.Models.AlgorithmsTreeView;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace GraphEditor.VMs
{
    class AlgorithmsVM : VMBase
    {
        readonly AlgorithmExecutor algorithmExecutor;
        public Graph<int>? Graph { get; private set; }
        public WeightedGraph<int, int>? WeightedGraph { get; private set; }
        string? start,
                destination,
                nodeDescription,
                algorithmOutput;
        ObservableCollection<AlgorithmsTreeViewItemBase> algorithmsTreeViewNodes;
        AlgorithmsTreeViewItemBase? algorithmsTreeViewSelectedNode;
        public string? Start
        {
            get => start;
            set
            {
                start = value;
                NotifyPropertyChanged(nameof(Start));
            }
        }
        public string? Destination
        {
            get => destination;
            set
            {
                destination = value;
                NotifyPropertyChanged(nameof(Destination));
            }
        }
        public string? NodeDescription
        {
            get => nodeDescription;
            set
            {
                nodeDescription = value;
                NotifyPropertyChanged(nameof(NodeDescription));
            }
        }
        public string? AlgorithmOutput
        {
            get => algorithmOutput;
            set
            {
                algorithmOutput = value;
                NotifyPropertyChanged(nameof(AlgorithmOutput));
            }
        }
        public ObservableCollection<AlgorithmsTreeViewItemBase> AlgorithmsTreeViewNodes
        {
            get => algorithmsTreeViewNodes;
            private set
            {
                algorithmsTreeViewNodes = value;
                NotifyPropertyChanged(nameof(AlgorithmsTreeViewNodes));
            }
        }
        public AlgorithmsTreeViewItemBase? AlgorithmsTreeViewSelectedNode
        {
            get => algorithmsTreeViewSelectedNode;
            set
            {
                algorithmsTreeViewSelectedNode = value;
                NodeDescription = algorithmsTreeViewSelectedNode!.Description;
            }
        }
        public ICommand ExecuteAlgorithmCommand { get; }
        public AlgorithmsVM()
        {
            algorithmExecutor = new(this);
            algorithmExecutor.AlgorithmExecuted += DisplayOnAlgorithmExecuted;
            ExecuteAlgorithmCommand = new ExecuteAlgorithmCommand(this);
            AlgorithmsTreeViewNodes = new()
            {
                new AlgorithmsTreeViewItemCategory
                {
                    Header = "Algorithms",
                    Children = new()
                    {
                        new AlgorithmsTreeViewItemCategory
                        {
                            Header = "Traversals",
                            Children = new()
                            {
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Depth first",
                                    Description = StringConstants.DepthFirstTraversalsDesc,
                                    AlgorithmInvoker = algorithmExecutor.ExecuteDFT,
                                    Children = null
                                },
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Breadth first",
                                    Description = StringConstants.BreadthFirstTraversalsDesc,
                                    Children = null
                                }
                            }
                        },
                        new AlgorithmsTreeViewItemCategory
                        {
                            Header = "Shortest paths",
                            Children = new()
                            {
                                new AlgorithmsTreeViewItemCategory
                                {
                                    Header = "Dijkstra",
                                    Children = new()
                                    {
                                        new AlgorithmsTreeViewItemAlgorithm
                                        {
                                            Header = "Paths' lengths",
                                            Description = StringConstants.DijkstraPathLengthsDesc,
                                            Children = null
                                        },
                                        new AlgorithmsTreeViewItemAlgorithm
                                        {
                                            Header = "Paths",
                                            Description = StringConstants.DijkstraPathsDesc,
                                            Children = null
                                        }
                                    }
                                },
                                new AlgorithmsTreeViewItemCategory
                                {
                                    Header = "A*",
                                    Children = new()
                                    {
                                        new AlgorithmsTreeViewItemAlgorithm
                                        {
                                            Header = "Path length",
                                            Description = StringConstants.AStarPathLengthDesc,
                                            Children = null
                                        },
                                        new AlgorithmsTreeViewItemAlgorithm
                                        {
                                            Header = "Path",
                                            Description = StringConstants.AStarPathDesc,
                                            Children = null
                                        }
                                    }
                                }
                            }
                        },
                        new AlgorithmsTreeViewItemCategory
                        {
                            Header = "Connectivity",
                            Children = new()
                            {
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Strong connected components",
                                    Description = StringConstants.StrongConnectedComponentsDesc,
                                    Children = null
                                },
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Is strong connected",
                                    Description = StringConstants.IsStrongConnectedDesc,
                                    Children = null
                                }
                            }
                        },
                        new AlgorithmsTreeViewItemCategory
                        {
                            Header = "Weight trees",
                            Children = new()
                            {
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Lowest cost weight tree",
                                    Description = StringConstants.LowestCostWeightTreeDesc,
                                    Children = null
                                },
                                new AlgorithmsTreeViewItemAlgorithm
                                {
                                    Header = "Highest cost weight tree",
                                    Description = StringConstants.HighestCostWeightTreeDesc,
                                    Children = null
                                }
                            }
                        }
                    }
                }
            };
        }
        public void ReactGraphSet(string _, Graph<int>? graph)
        {
            WeightedGraph = null;
            Graph = graph;
        }
        public void ReactWeightedGraphSet(string _, WeightedGraph<int, int> weightedGraph)
        {
            Graph = null;
            WeightedGraph = weightedGraph;
        }
        void DisplayOnAlgorithmExecuted(string output) => AlgorithmOutput = output;
    }
}
