using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandDispatcher
{
    class ExampleCommandDispatcher : IExampleCommandDispatcher
    {
        private readonly IEnumerable<ICommandHandler<IExampleCommandDispatcher>> _handlers;

        public ExampleCommandDispatcher(IEnumerable<ICommandHandler<IExampleCommandDispatcher>> handlers)
        {
            _handlers = handlers;
        }

        public Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : BaseCommand
            where TResult : BaseResult
        {
            var handler = _handlers
                .Where(handler => typeof(ICommandHandler<TCommand, TResult>).IsAssignableFrom(handler.GetType()))
                .Cast<ICommandHandler<TCommand, TResult>>()
                .Single();

            return handler.Handle(command);
        }
    }
}
