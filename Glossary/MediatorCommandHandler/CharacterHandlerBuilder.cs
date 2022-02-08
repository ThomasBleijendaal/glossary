using MediatorCommandHandler.Characters;
using MediatorCommandHandler.Handlers;
using MediatorCommandHandler.Implements;
using MediatorCommandHandler.Weapons;
using MediatR;

namespace MediatorCommandHandler;

internal class CharacterHandlerBuilder : ICharacterHandlerBuilder
{
    private readonly IMediator _mediator;

    public CharacterHandlerBuilder(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public ICharacterHandler BuildCharacters(CharacterDetails details)
    {
        return details switch
        {
            WarriorCharacter warrior => new CharacterHandler(
                "Warrior",
                _mediator,
                Random.Shared.Next(180, 240),
                new Sword(warrior.Strength),
                new Voice(1)),

            WizardCharacter wizard => new CharacterHandler(
                "Wizard",
                _mediator,
                Random.Shared.Next(80, 120),
                new Melee(5),
                Random.Shared.Next(1, 4) switch
                {
                    1 => new Staff(wizard.SpellPower),
                    2 => new Fan(wizard.SpellPower),
                    3 => new Rod(wizard.SpellPower),
                    _ => throw new InvalidOperationException()
                }),

            BardCharacter bard => new CharacterHandler(
                "Bard", 
                _mediator,
                Random.Shared.Next(70, 90), 
                new Melee(Random.Shared.Next(4, 6)), 
                new Whistle(bard.MusicPower)),

            _ => throw new NotImplementedException()
        };
    }
}
