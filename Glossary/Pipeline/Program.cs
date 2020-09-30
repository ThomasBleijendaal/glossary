/**
* Using TPL Dataflow a pipeline can be made of consecutive blocks which process parts in incoming
* stream of work with full control of parallelism, fanning out and in and batching.
* 
* Example pipeline:
* 
* Incoming data: [1, 1, 1, 1, 1, 1, 1, ..]
* 
* Multiplier: x2, 1s, max 5 parallel
* Batcher.
* Aggregator: Sum 2 values, .5s, max 2 parallel 
* Buffer.
* Outputer: Console write values
*/

using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Pipeline
{
    class Program : BaseProgram
    {
        static async Task Main(string[] args)
        {
            await Init<Program, PipelineApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddTransient<IPipelineActions, ExamplePipelineActions>();
            services.AddSingleton<IPipelineFactory, PipelineFactory>();
        }

        public class PipelineApp : BaseApp
        {
            private readonly IPipelineFactory _pipelineFactory;

            public PipelineApp(IPipelineFactory pipelineFactory)
            {
                _pipelineFactory = pipelineFactory;
            }

            public override async Task Run()
            {
                var pipeline = _pipelineFactory.BuildPipeline();

                foreach (var input in Enumerable.Repeat(1, 20))
                {
                    pipeline.Input.Post(input);
                }

                // this stops the input from receiving more data, and has all the blocks propagate their completion
                pipeline.Input.Complete();

                await pipeline.Completion;
            }
        }
    }
}
