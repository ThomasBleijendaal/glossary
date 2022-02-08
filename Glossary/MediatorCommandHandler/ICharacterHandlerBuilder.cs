using MediatorCommandHandler.Characters;
using MediatorCommandHandler.Handlers;

namespace MediatorCommandHandler;

internal interface ICharacterHandlerBuilder
{
    ICharacterHandler BuildCharacters(CharacterDetails details);
}
