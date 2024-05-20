using GraphEditor.Commands;
using GraphEditor.Commands.WindowManipulationCommands;
using GraphEditor.Windows;
namespace GraphEditor.VMs
{
    class MainVM : VMBase
    {
        public ObserverVM ObserverVM { get; }
        public ActionsVM ActionsVM { get; }
        public ExplorerVM ExplorerVM { get; }
        public AlgorithmsVM AlgorithmsVM { get; }
        public OpenDialogWindowCommand<NewGraphPromptWindow> OpenNewGraphPromptWindowCommand { get; }
        public OpenGraphFromFileCommand OpenGraphFromFileCommand { get; }
        public MainVM()
        {
            ObserverVM = new();
            ActionsVM = new();
            ExplorerVM = new();
            AlgorithmsVM = new();
            ExplorerVM.GraphSelected += ObserverVM.ReactGraphSet;
            ExplorerVM.WeightedGraphSelected += ObserverVM.ReactWeightedGraphSet;
            ExplorerVM.GraphSelected += ActionsVM.ReactGraphSet;
            ExplorerVM.WeightedGraphSelected += ActionsVM.ReactWeightedGraphSet;
            ExplorerVM.GraphSelected += AlgorithmsVM.ReactGraphSet;
            ExplorerVM.WeightedGraphSelected += AlgorithmsVM.ReactWeightedGraphSet;
            ExplorerVM.CloseGraphCommand.GraphClosed += ExplorerVM.ReactGraphClosed;
            ActionsVM.AddVertexCommand.VertexAdded += ObserverVM.ReactVertexAdded;
            ActionsVM.RemoveVertexCommand.VertexRemoved += ObserverVM.ReactVertexRemoved;
            ActionsVM.ConnectCommand.Connected += ObserverVM.ReactEdgesChanged;
            ActionsVM.DisconnectCommand.Disconnected += ObserverVM.ReactEdgesChanged;
            ObserverVM.GraphUpdated += ExplorerVM.ReactGraphUpdated;
            OpenNewGraphPromptWindowCommand = new();
            OpenGraphFromFileCommand = new();
            OpenGraphFromFileCommand.AttemptToOpenNamedGraph += ExplorerVM.VerifyGraphNameAllowed;
            OpenGraphFromFileCommand.GraphOpened += ExplorerVM.ReactGraphOpened;
            OpenGraphFromFileCommand.WeightedGraphOpened += ExplorerVM.ReactWeightedGraphOpened;
        }
    }
}
