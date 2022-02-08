using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Weapons;

namespace MediatorCommandHandler.Requests;

internal record MeleeAttack(Melee RequestModel) : Request<Melee, MeleeDamage>(RequestModel);
