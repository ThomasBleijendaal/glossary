using System;

namespace Factory
{
    public class ElectricEngine : IEngine
    {
        public void TurnOff() => Console.WriteLine("The coil whine stops.");

        public void TurnOn() => Console.WriteLine("A soft coil whine indicates that the frequency controller is online.");
    }
}
