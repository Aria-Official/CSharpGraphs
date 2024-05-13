using GraphEditor.VMs;
using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}
