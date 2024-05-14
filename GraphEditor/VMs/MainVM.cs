using GraphEditor.Commands;
using GraphEditor.Windows;
using System.Windows.Input;
namespace GraphEditor.VMs
{
    class MainVM : VMBase
    {
        public GraphObserverVM GraphObserverVM { get; }
        public GraphOptionsVM GraphOptionsVM { get; }
        public GraphExplorerVM GraphExplorerVM { get; }
        public OpenDialogWindowCommand<NewGraphPromptWindow> OpenNewGraphPromptWindowCommand { get; }
        public ICommand OpenGraphFromFileCommand { get; }
        public ICommand SaveGraphAsFileCommand { get; }
        public MainVM()
        {
            GraphObserverVM = new();
            GraphOptionsVM = new();
            GraphExplorerVM = new();
            GraphExplorerVM.GraphSelected += GraphObserverVM.ReactGraphSet;
            GraphExplorerVM.WeightedGraphSelected += GraphObserverVM.ReactWeightedGraphSet;
            GraphExplorerVM.GraphSelected += GraphOptionsVM.ReactGraphSet;
            GraphExplorerVM.WeightedGraphSelected += GraphOptionsVM.ReactWeightedGraphSet;
            OpenNewGraphPromptWindowCommand = new();
            OpenGraphFromFileCommand = null;
            SaveGraphAsFileCommand = null;
        }
    }
}
