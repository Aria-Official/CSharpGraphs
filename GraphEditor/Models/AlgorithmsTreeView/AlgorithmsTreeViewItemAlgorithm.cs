using System;
namespace GraphEditor.Models.AlgorithmsTreeView
{
    class AlgorithmsTreeViewItemAlgorithm : AlgorithmsTreeViewItemBase
    {
        public Action? AlgorithmInvoker { get; init; }
        public AlgorithmsTreeViewItemAlgorithm() { }
    }
}
