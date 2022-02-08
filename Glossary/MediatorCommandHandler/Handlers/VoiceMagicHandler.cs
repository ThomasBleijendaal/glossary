using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class VoiceMagicHandler : IRequestHandler<VoiceMagic, Damage>
{
    public Task<Damage> Handle(VoiceMagic request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Damage>(new AirDamage(request.RequestModel.Level));
    }
}
