/**
 * A visitor is a class that contains logic to process elements which allow the visitor
 * to visit. This separates the logic and the data.
 * 
 * Key point is that the element should accept a certain visitor and that the accept method
 * should pick the correct method in the visitor.
 */

using System;
using System.Threading.Tasks;
using ZCommon;

namespace Visitor
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, VisitorApp>();
        }

        public class VisitorApp : BaseApp
        {
            public override Task Run()
            {
                var treeElements =
                new Folder("Root", 0,
                    new Folder("Folder 1", 0,
                        new File("File 1", 12),
                        new File("File 2", 34)
                    ),
                    new Folder("Folder 2", 0,
                        new Folder("Folder 3", 0,
                            new File("File 3", 56),
                            new File("File 4", 78)
                        )
                    )
                );

                var printer = new TreePrinter();
                var accumulator = new TreeAccumulator();

                treeElements.Accept(printer);
                treeElements.Accept(accumulator);

                Console.WriteLine($"Total size: {accumulator.Size} B");

                return Task.CompletedTask;
            }
        }
    }
}
