namespace Visitor
{
    public class TreeAccumulator : ITreeElementVisitor
    {
        public int Size { get; private set; }

        public void Visit(Folder element)
        {
            foreach (var child in element.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(File element)
        {
            Size += element.Size;
        }
    }
}
