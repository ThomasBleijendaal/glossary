using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Weapons;

internal record Melee(int Fingers) : Weapon
{
    public override IRequest<MeleeDamage> Request => new MeleeAttack(this);
};
