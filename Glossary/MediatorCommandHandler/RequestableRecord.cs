using MediatR;

namespace MediatorCommandHandler;

internal abstract record RequestableRecord<TResponse>
{
    public abstract IRequest<TResponse> Request { get; }
}
