using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class GraphExplorerView : UserControl
    {
        public GraphExplorerView()
        {
            InitializeComponent();
            for (int i = 0; i < 3;)
                ExplorerDataGrid.Columns[i++].Width = new DataGridLength(0.33, DataGridLengthUnitType.Star);
        }
    }
}
