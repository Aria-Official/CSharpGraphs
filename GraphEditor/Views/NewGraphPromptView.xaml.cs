using GraphEditor.VMs;
using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class NewGraphPromptView : UserControl
    {
        public NewGraphPromptView()
        {
            InitializeComponent();
            DataContext = new NewGraphPromptVM();
        }
    }
}
