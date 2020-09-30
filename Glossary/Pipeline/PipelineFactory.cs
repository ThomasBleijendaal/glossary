using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;

namespace Pipeline
{
    public class PipelineFactory : IPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PipelineFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Pipeline<int> BuildPipeline()
        {
            // resolving the correct actions can of course be dependent on some logic given to this method
            var pipelineActions = _serviceProvider.GetRequiredService<IPipelineActions>();

            var multiplier = new TransformBlock<int, int>(pipelineActions.Multiplier, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 5 });

            var batcher = new BatchBlock<int>(2);

            var aggregator = new TransformBlock<IEnumerable<int>, int>(pipelineActions.Aggregator, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 2 });

            var buffer = new BufferBlock<int>();

            var outputer = new ActionBlock<int>(pipelineActions.Outputer);

            var propagateCompletion = new DataflowLinkOptions { PropagateCompletion = true };

            multiplier.LinkTo(batcher, propagateCompletion);
            batcher.LinkTo(aggregator, propagateCompletion);
            aggregator.LinkTo(buffer, propagateCompletion);
            buffer.LinkTo(outputer, propagateCompletion);

            return new Pipeline<int>
            {
                Input = multiplier,
                Completion = outputer.Completion
            };
        }
    }
}
