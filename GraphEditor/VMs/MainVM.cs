namespace GraphEditor.VMs
{
    class MainVM : VMBase
    {
        GraphObserverVM? graphObserverVM;
        public GraphObserverVM? GraphObserverVM
        {
            get => graphObserverVM;
            set
            {
                graphObserverVM = value;
                NotifyPropertyChanged(nameof(GraphObserverVM));
            }
        }
        public GraphOptionsVM GraphOptionsVM { get; }
        public GraphExplorerVM GraphExplorerVM { get; }
        public MainVM()
        {
            GraphOptionsVM = new(null, null, null, null, null, null);
            GraphExplorerVM = new();
        }
    }
}
