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
                InputParser.ParseVertex(algorithmsVM.Start, out int vertex,
                                        "Start vertex was not specified.",
                                        "Start vertex doesn't parse to an integer.");
                IEnumerable<int> DFT;
                if (algorithmsVM.Graph is not null) DFT = Traversals.DepthFirstTraversal(algorithmsVM.Graph, vertex);
                else DFT = Traversals.DepthFirstTraversal(algorithmsVM.WeightedGraph!, vertex);
                AlgorithmExecuted?.Invoke(string.Join(' ', DFT));
            }
            catch (InvalidInputException IIe) { MessageBox.Show(IIe.Message, "Input error"); }
            catch (InvalidOperationException IOe) { MessageBox.Show(IOe.Message, "Missing vertex"); }
        }
    }
}
