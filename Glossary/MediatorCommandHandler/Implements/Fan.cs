using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Implements;

internal record Fan(int Level) : Implement
{
    public override IRequest<Damage> Request => new FanMagic(this);
}
