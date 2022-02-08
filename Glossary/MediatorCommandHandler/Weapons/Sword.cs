using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Requests;
using MediatR;

namespace MediatorCommandHandler.Weapons;

internal record Sword(int Level) : Weapon
{
    public override IRequest<MeleeDamage> Request => new SwordAttack(this);
}
