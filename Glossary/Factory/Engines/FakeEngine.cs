using System;

namespace Factory.Engines
{
    public class FakeEngine : IEngine
    {
        public void TurnOff() => Console.WriteLine("The little speaker stops buzzing.");

        public void TurnOn() => Console.WriteLine("A little speaker starts to buzz like an engine.");
    }
}
