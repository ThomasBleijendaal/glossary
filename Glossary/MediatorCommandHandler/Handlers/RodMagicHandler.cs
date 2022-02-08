using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class RodMagicHandler : IRequestHandler<RodMagic, Damage>
{
    public Task<Damage> Handle(RodMagic request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Damage>(new LightningDamage(request.RequestModel.Level));
    }
}
