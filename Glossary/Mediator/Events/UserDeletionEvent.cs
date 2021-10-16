namespace Mediator.Events
{
    public class UserDeletionEvent : IMediationEvent
    {
        public string UserId { get; init; } = default!;
    }
}
