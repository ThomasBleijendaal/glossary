using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Implements;

internal record Whistle(int Level) : Implement
{
    public override IRequest<Damage> Request => new WhistleMagic(this);
}
