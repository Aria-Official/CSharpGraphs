using System;
namespace GraphEditor.Models.CustomEventArgs
{
    class NameEventArgs : EventArgs
    {
        public string Name { get; }
        public NameEventArgs(string name) => Name = name;
    }
}
