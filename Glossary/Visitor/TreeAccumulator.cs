namespace Visitor
{
    public class TreeAccumulator : ITreeElementVisitor
    {
        public int Size { get; private set; }

        public void Visit(Folder element)
        {
            
        }

        public void Visit(File element)
        {
            Size += element.Size;
        }
    }
}
