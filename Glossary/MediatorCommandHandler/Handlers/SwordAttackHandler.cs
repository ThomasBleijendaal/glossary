using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class SwordAttackHandler : IRequestHandler<SwordAttack, MeleeDamage>
{
    public Task<MeleeDamage> Handle(SwordAttack request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new MeleeDamage(request.RequestModel.Level * 2));
    }
}
