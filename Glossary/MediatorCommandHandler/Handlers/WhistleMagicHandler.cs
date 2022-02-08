using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class WhistleMagicHandler : IRequestHandler<WhistleMagic, Damage>
{
    public Task<Damage> Handle(WhistleMagic request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Damage>(new AirDamage(request.RequestModel.Level));
    }
}
