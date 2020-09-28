using System.Collections.Generic;

namespace Visitor
{
    public interface ITreeElement
    {
        string Name { get; set; }

        int Size { get; set; }

        ITreeElement Parent { get; set; }

        IEnumerable<ITreeElement> Children { get; }

        void Accept(ITreeElementVisitor visitor);
    }
}
