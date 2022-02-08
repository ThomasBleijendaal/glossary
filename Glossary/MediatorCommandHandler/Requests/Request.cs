using MediatR;

namespace MediatorCommandHandler.Requests;

internal abstract record Request<TRequest, TResponse>(TRequest RequestModel) : IRequest<TResponse>;
