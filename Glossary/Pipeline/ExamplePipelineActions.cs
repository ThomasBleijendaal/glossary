using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pipeline
{
    public class ExamplePipelineActions : IPipelineActions
    {
        public async Task<int> Aggregator(IEnumerable<int> input)
        {
            Console.WriteLine("Aggregator start");

            var result = input.Sum();

            await Task.Delay(500);

            Console.WriteLine("Aggregator complete");

            return result;
        }

        public async Task<int> Multiplier(int input)
        {
            Console.WriteLine("Multiplier start");

            var result = input * 2;

            await Task.Delay(1000);

            Console.WriteLine("Multiplier complete");

            return result;
        }

        public Task Outputer(int input)
        {
            Console.WriteLine($"Ouputer: {input}");

            return Task.CompletedTask;
        }
    }
}
