using GraphEditor.Stores;
using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = MainVMStore.MainVM;
        }
    }
}
