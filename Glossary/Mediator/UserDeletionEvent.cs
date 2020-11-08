namespace Mediator
{
    public class UserDeletionEvent : IMediationEvent
    {
        public string UserId { get; init; } = default!;
    }
}
