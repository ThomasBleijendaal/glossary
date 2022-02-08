using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class StaffMagicHandler : IRequestHandler<StaffMagic, Damage>
{
    public Task<Damage> Handle(StaffMagic request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Damage>(new FireDamage(request.RequestModel.Level));
    }
}
