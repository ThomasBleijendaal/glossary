using MediatorCommandHandler.Handlers;

namespace MediatorCommandHandler;

internal static class InteractionHelper
{
    public static bool IsInThreadRange(ICharacterHandler source, ICharacterHandler target)
    {
        return Math.Abs(source.Position.X - target.Position.X) < 2 && Math.Abs(source.Position.Y - target.Position.Y) < 2;
    }
}
