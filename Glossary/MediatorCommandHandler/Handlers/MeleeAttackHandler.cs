using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class MeleeAttackHandler : IRequestHandler<MeleeAttack, MeleeDamage>
{
    public Task<MeleeDamage> Handle(MeleeAttack request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new MeleeDamage(request.RequestModel.Fingers));
    }
}
