using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Implements;

internal record Staff(int Level) : Implement
{
    public override IRequest<Damage> Request => new StaffMagic(this);
}
