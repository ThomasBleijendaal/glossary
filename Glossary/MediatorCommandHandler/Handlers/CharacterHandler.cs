using MediatorCommandHandler.Damages;
using MediatorCommandHandler.Implements;
using MediatorCommandHandler.Weapons;
using MediatR;

namespace MediatorCommandHandler.Handlers;

internal class CharacterHandler : ICharacterHandler
{
    private readonly IMediator _mediator;
    private readonly Weapon _weapon;
    private readonly Implement _implement;

    public CharacterHandler(
        string name,
        IMediator mediator,
        int health,
        Weapon weapon,
        Implement implement)
    {
        Name = name;
        _mediator = mediator;
        Health = health;
        _weapon = weapon;
        _implement = implement;

        Walk();
    }

    public string Name { get; }
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    public Position Position { get; private set; }

    public async Task<MeleeDamage> AttackAsync()
    {
        var attack = await _mediator.Send(_weapon.Request);

        Console.WriteLine($"{Name} going to do {attack} with {_weapon.GetType().Name}");

        return attack;
    }

    public void Defend(MeleeDamage attack)
    {
        Console.WriteLine($"{Name} takes {attack}");

        Health -= attack.DamagePoints;
        Console.WriteLine($"{Name} HP is {Health}");
    }

    public async Task<Damage> DoMagicAsync()
    {
        var attack = await _mediator.Send(_implement.Request);

        Console.WriteLine($"{Name} going to do {attack} using {_implement.GetType().Name}");

        return attack;
    }

    public void ReceiveMagic(Damage effect)
    {
        if (effect is FireDamage fireMagic)
        {
            Console.WriteLine($"{Name} get burnt with {fireMagic.DamagePoints} fire damage");
            Health -= fireMagic.DamagePoints;
        }
        else if (effect is AirDamage airMagic)
        {
            Console.WriteLine($"{Name} get blown with {airMagic.DamagePoints} air damage");
            Health -= airMagic.DamagePoints;
        }
        else if (effect is LightningDamage lightningMagic)
        {
            Console.WriteLine($"{Name} get zapped with {lightningMagic.DamagePoints} lightning damage");
            Health -= lightningMagic.DamagePoints;
        }

        Console.WriteLine($"{Name} HP is {Health}");
    }

    public void Walk()
    {
        Position = Position with
        {
            X = Position.X + Random.Shared.Next(-Position.X, 5),
            Y = Position.Y + Random.Shared.Next(-Position.Y, 5)
        };

        Console.WriteLine($"{Name} moves to {Position}");
    }
}
