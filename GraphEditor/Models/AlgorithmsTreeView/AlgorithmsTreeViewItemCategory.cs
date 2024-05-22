using System.Collections.ObjectModel;
namespace GraphEditor.Models.AlgorithmsTreeView
{
    class AlgorithmsTreeViewItemCategory : AlgorithmsTreeViewItemBase
    {
        public ObservableCollection<AlgorithmsTreeViewItemBase>? Children { get; init; }
        public AlgorithmsTreeViewItemCategory() { }
    }
}
