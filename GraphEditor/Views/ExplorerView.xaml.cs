using System.Windows.Controls;
namespace GraphEditor.Views
{
    public partial class ExplorerView : UserControl
    {
        public ExplorerView()
        {
            InitializeComponent();
            for (int i = 0; i < 3;)
                ExplorerDataGrid.Columns[i++].Width = new DataGridLength(0.33, DataGridLengthUnitType.Star);
        }
    }
}
