using System;
namespace GraphEditor.Models.AlgorithmsTreeView
{
    class AlgorithmsTreeViewItemAlgorithm : AlgorithmsTreeViewItemBase
    {
        public Action? Execution { get; init; }
        public string? Description { get; init; }
        public AlgorithmsTreeViewItemAlgorithm() { }
    }
}
