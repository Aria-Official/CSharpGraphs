using GraphEditor.VMs;
using Microsoft.Win32;
using System.IO;
namespace GraphEditor.Commands
{
    class SaveOutputToTextFileCommand : SyncCommand
    {
        readonly AlgorithmsVM algorithmsVM;
        public SaveOutputToTextFileCommand(AlgorithmsVM algorithmsVM) =>
            this.algorithmsVM = algorithmsVM;
        public override void Execute(object? parameter)
        {
            SaveFileDialog dialog = new()
            {
                FileName = "Output",
                DefaultExt = ".txt",
                Filter = "Text files(.txt)|*.txt|all Files(*.*)|*.*",
                CreatePrompt = true,
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() != true) return; // Returns 'bool?' so explicit comparison is required.
            string path = Path.GetFullPath(dialog.FileName);
            File.WriteAllText(path, algorithmsVM.AlgorithmOutput);
        }
    }
}
