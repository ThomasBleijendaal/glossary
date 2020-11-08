namespace Mediator
{
    public class OrderCompletionEvent : IMediationEvent
    {
        public string UserId { get; init; } = default!;
    }
}
