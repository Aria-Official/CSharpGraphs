using GraphEditor.Commands;
using System.Windows.Input;
namespace GraphEditor.VMs
{
    class NewGraphPromptVM : VMBase
    {
        string? graphName;
        bool weighted;
        public string? GraphName
        {
            get => graphName;
            set
            {
                graphName = value;
                NotifyPropertyChanged(nameof(GraphName));
            }
        }
        public bool Weighted
        {
            get => weighted;
            set
            {
                weighted = value;
                NotifyPropertyChanged(nameof(Weighted));
            }
        }
        public ICommand CreateNewGraphCommand { get; set; }
        public ICommand CloseWindow { get; set; }
        public NewGraphPromptVM()
        {
            CreateNewGraphCommand = new TryCreateNewGraphCommand(this);
            CloseWindow = new CloseLastDialogWindowCommand();
        }
    }
}
