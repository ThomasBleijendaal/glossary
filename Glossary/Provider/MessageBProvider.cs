namespace Provider
{
    public class MessageBProvider : ILogMessageProvider
    {
        public string CreateMessage(string name) => $"Hello again, {name}.";
    }
}
