namespace Visitor
{
    public interface ITreeElementVisitor
    {
        void Visit(Folder element);

        void Visit(File element);
    }
}
