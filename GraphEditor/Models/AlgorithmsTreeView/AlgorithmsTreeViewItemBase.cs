using System.Collections.ObjectModel;
namespace GraphEditor.Models.AlgorithmsTreeView
{
    public abstract class AlgorithmsTreeViewItemBase
    {
        public string? Header { get; set; }
        public string? Description { get; set; }
        public ObservableCollection<AlgorithmsTreeViewItemBase>? Children { get; init; }
        public AlgorithmsTreeViewItemBase() { }
    }
}
