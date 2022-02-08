using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Implements;

internal record Rod(int Level) : Implement
{
    public override IRequest<Damage> Request => new RodMagic(this);
}
