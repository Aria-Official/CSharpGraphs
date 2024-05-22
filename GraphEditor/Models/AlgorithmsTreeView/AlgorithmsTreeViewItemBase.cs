using System.Collections.ObjectModel;
namespace GraphEditor.Models.AlgorithmsTreeView
{
    public abstract class AlgorithmsTreeViewItemBase
    {
        public string? Header { get; init; }
        public AlgorithmsTreeViewItemBase() { }
    }
}
