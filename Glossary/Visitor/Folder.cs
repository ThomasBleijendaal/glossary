using System;
using System.Collections.Generic;

namespace Visitor
{
    public class Folder : ITreeElement
    {
        public Folder(string name, int size, params ITreeElement[] children)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Size = size;
            Children = children ?? throw new ArgumentNullException(nameof(children));

            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public ITreeElement Parent { get; set; }

        public IEnumerable<ITreeElement> Children { get; }

        public void Accept(ITreeElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
