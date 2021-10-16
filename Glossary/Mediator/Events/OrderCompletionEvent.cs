namespace Mediator.Events
{
    public class OrderCompletionEvent : IMediationEvent
    {
        public string UserId { get; init; } = default!;
    }
}
