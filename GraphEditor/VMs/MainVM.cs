using GraphEditor.Commands.WindowManipulationCommands;
using GraphEditor.Windows;
using System.Windows.Input;
namespace GraphEditor.VMs
{
    class MainVM : VMBase
    {
        public ObserverVM ObserverVM { get; }
        public ActionsVM ActionsVM { get; }
        public ExplorerVM ExplorerVM { get; }
        public OpenDialogWindowCommand<NewGraphPromptWindow> OpenNewGraphPromptWindowCommand { get; }
        public ICommand OpenGraphFromFileCommand { get; }
        public ICommand SaveGraphAsFileCommand { get; }
        public MainVM()
        {
            ObserverVM = new();
            ActionsVM = new();
            ExplorerVM = new();
            ExplorerVM.GraphSelected += ObserverVM.ReactGraphSet;
            ExplorerVM.WeightedGraphSelected += ObserverVM.ReactWeightedGraphSet;
            ExplorerVM.GraphSelected += ActionsVM.ReactGraphSet;
            ExplorerVM.WeightedGraphSelected += ActionsVM.ReactWeightedGraphSet;
            ActionsVM.AddVertexCommand.VertexAdded += ObserverVM.DisplayOnVertexAdded;
            ActionsVM.RemoveVertexCommand.VertexRemoved += ObserverVM.DisplayOnVertexRemoved;
            ActionsVM.ConnectCommand.Connected += ObserverVM.DisplayOnEdgesChanged;
            ActionsVM.DisconnectCommand.Disconnected += ObserverVM.DisplayOnEdgesChanged;
            ObserverVM.GraphUpdated += ExplorerVM.ReactGraphUpdated;
            OpenNewGraphPromptWindowCommand = new();
            OpenGraphFromFileCommand = null;
            SaveGraphAsFileCommand = null;
        }
    }
}
