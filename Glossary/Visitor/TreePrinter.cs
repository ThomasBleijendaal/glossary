using System;

namespace Visitor
{
    public class TreePrinter : ITreeElementVisitor
    {
        public void Visit(File element)
        {
            Console.WriteLine($"{Depth(element)} File: {element.Name} - {element.Size} bytes");
        }

        public void Visit(Folder element)
        {
            Console.WriteLine($"{Depth(element)} Folder: {element.Name}");
        }

        private string Depth(ITreeElement element)
        {
            if (element.Parent != null)
            {
                return $"-{Depth(element.Parent)}";
            }

            return "";
        }
    }
}
