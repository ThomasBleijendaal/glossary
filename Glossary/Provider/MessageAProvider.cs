namespace Provider
{
    public class MessageAProvider : ILogMessageProvider
    {
        public string CreateMessage(string name) => $"Hi {name}!";
    }
}
