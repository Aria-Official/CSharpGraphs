using GraphEditor.VMs;
namespace GraphEditor.Stores
{
    static class MainVMStore
    {
        public static readonly MainVM MainVM;
        static MainVMStore() => MainVM = new();
    }
}
