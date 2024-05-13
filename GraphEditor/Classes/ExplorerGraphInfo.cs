namespace GraphEditor.Classes
{
    class ExplorerGraphInfo
    {
        public string Name { get; set; }
        public bool Saved { get; set; }
        public GraphType GraphType { get; set; }
        public ExplorerGraphInfo(string name, GraphType graphType)
        {
            Name = name;
            GraphType = graphType;
        }
    }
}
