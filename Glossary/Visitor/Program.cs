using System;
using System.Collections;
using System.Collections.Generic;

namespace Visitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public interface ITreeElement
    {
        string Name { get; set; }

        int Size { get; set; }

        ITreeElement Parent { get; }

        IEnumerable<ITreeElement> Children { get; }

        void Accept(ITreeElementVisitor visitor);
    }

    public class TreeElement : ITreeElement
    {
        public string Name { get; set; }
        
        public int Size { get; set; }

        public ITreeElement Parent { get; }
        
        public IEnumerable<ITreeElement> Children { get; }

        public void Accept(ITreeElementVisitor visitor)
        {

        }
    }

    public interface ITreeElementVisitor
    {
        void PrintFolder(ITreeElement element);

        void PrintFile(ITreeElement element);
    }

    public class TreePrinter : ITreeElementVisitor
    {
        public void PrintFile(ITreeElement element)
        {
            Console.WriteLine($"File: {element.Name} - {element.Size} bytes");
        }

        public void PrintFolder(ITreeElement element)
        {
            Console.WriteLine($"Folder: {element.Name}");
        }
    }
}
