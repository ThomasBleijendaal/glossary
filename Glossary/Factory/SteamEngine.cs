using System;

namespace Factory
{
    public class SteamEngine : IEngine
    {
        public void TurnOff() => Console.WriteLine("The engine vents its steam and with lots of hisses and puffs finally becomes silent.");

        public void TurnOn() => Console.WriteLine("The fire under the steam vessel heats up the water and after quite some time the steam pressure is at a useable level.");
    }
}
