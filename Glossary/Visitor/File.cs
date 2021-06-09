using System;
using System.Collections.Generic;
using System.Linq;

namespace Visitor
{
    public class File : ITreeElement
    {
        public File(string name, int size)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Size = size;
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public ITreeElement Parent { get; set; }

        public IEnumerable<ITreeElement> Children { get; } = Enumerable.Empty<ITreeElement>();

        public void Accept(ITreeElementVisitor visitor)
        {
            visitor.Visit(this);

            foreach (var child in Children)
            {
                child.Accept(visitor);
            }
        }
    }
}
