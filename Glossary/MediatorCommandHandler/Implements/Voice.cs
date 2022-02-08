using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Implements;

internal record Voice(int Level) : Implement
{
    public override IRequest<Damage> Request => new VoiceMagic(this);
}
