using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Weapons;

namespace MediatorCommandHandler.Requests;

internal record SwordAttack(Sword RequestModel) : Request<Sword, MeleeDamage>(RequestModel);
