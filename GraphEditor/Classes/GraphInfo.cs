namespace GraphEditor.Classes
{
    class GraphInfo
    {
        public string Name { get; set; }
        public bool Saved { get; set; }
        public GraphType GraphType { get; set; }
        public GraphInfo(string name, GraphType graphType)
        {
            Name = name;
            GraphType = graphType;
        }
    }
}
