using GraphEditor.Models.AlgorithmsTreeView;
using GraphEditor.VMs;
using System.Windows;
using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class AlgorithmsView : UserControl
    {
        public AlgorithmsView() => InitializeComponent();
        void AlgorithmsTreeViewHandleSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var vm = DataContext as AlgorithmsVM;
            if (vm is not null) vm.AlgorithmsTreeViewSelectedNode = (AlgorithmsTreeViewItemBase)e.NewValue;
        }
    }
}
