using GraphEditor.VMs;
namespace GraphEditor.Commands
{
    class SaveOutputToTextFileCommand : SyncCommand
    {
        readonly AlgorithmsVM algorithmsVM;
        public SaveOutputToTextFileCommand(AlgorithmsVM algorithmsVM) =>
            this.algorithmsVM = algorithmsVM;
        public override void Execute(object? parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}
