using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class FanMagicHandler : IRequestHandler<FanMagic, Damage>
{
    public Task<Damage> Handle(FanMagic request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Damage>(new AirDamage(request.RequestModel.Level));
    }
}
