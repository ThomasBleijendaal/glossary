using MediatorCommandHandler.Damages;

namespace MediatorCommandHandler.Handlers;

internal interface ICharacterHandler
{
    Position Position { get; }

    bool IsAlive { get; }

    string Name { get; }

    void Walk();

    Task<MeleeDamage> AttackAsync();

    void Defend(MeleeDamage attack);

    Task<Damage> DoMagicAsync();

    void ReceiveMagic(Damage effect);

}
