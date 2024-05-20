using GraphEditor.Models.AlgorithmsTreeView;
using GraphEditor.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor.Views
{
    /// <summary>
    /// Interaction logic for AlgorithmsView.xaml
    /// </summary>
    public partial class AlgorithmsView : UserControl
    {
        public AlgorithmsView()
        {
            InitializeComponent();
        }
        void AlgorithmsTreeViewHandleSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var vm = DataContext as AlgorithmsVM;
            if (vm is not null) vm.AlgorithmsTreeViewSelectedNode = (AlgorithmsTreeViewItemBase)e.NewValue;
        }
    }
}
