using MediatorCommandHandler.Damages;

namespace MediatorCommandHandler.Weapons;

internal abstract record Weapon() : RequestableRecord<MeleeDamage>;
