using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Pipeline
{
    /**
     * Using TPL Dataflow a pipeline can be made of consecutive blocks which process parts in incoming
     * stream of work with full control of parallelism, fan out and in and batching.
     * 
     * Example pipeline:
     * 
     * Incoming data: [1, 1, 1, 1, 1, 1, 1, ..]
     * 
     * Mutation 1: x2, 1s, max 5 parallel
     * Batcher.
     * Mutation 2: Sum 2 values, .5s, max 2 parallel 
     * Buffer.
     * Outputer: Console write values
     */

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var mutator1 = new TransformBlock<int, int>(async input =>
            {
                Console.WriteLine("mutator1 start");

                var result = input * 2;

                await Task.Delay(1000);

                Console.WriteLine("mutator1 complete");

                return result;
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 });

            var batcher = new BatchBlock<int>(2);

            var mutator2 = new TransformBlock<IEnumerable<int>, int>(async inputs =>
            {
                Console.WriteLine("mutator2 start");

                var result = inputs.Sum();

                await Task.Delay(500);

                Console.WriteLine("mutator2 complete");

                return result;
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });

            var buffer = new BufferBlock<int>();

            var outputer = new ActionBlock<int>(Console.WriteLine);

            mutator1.LinkTo(batcher);
            batcher.LinkTo(mutator2);
            mutator2.LinkTo(buffer);
            buffer.LinkTo(outputer);

            foreach (var input in Enumerable.Repeat(1, 20))
            {
                mutator1.Post(input);
            }

            await outputer.Completion;
        }
    }
}
