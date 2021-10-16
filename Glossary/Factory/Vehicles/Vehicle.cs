using System;
using Factory.Behaviours;
using Factory.Engines;

namespace Factory.Vehicles
{
    public class Vehicle : IVehicle
    {
        private readonly IEngine _engine;
        private readonly IVehicleBehaviour _vehicleBehaviour;

        public Vehicle(IEngine engine, IVehicleBehaviour vehicleBehaviour)
        {
            _engine = engine;
            _vehicleBehaviour = vehicleBehaviour;
        }

        public void MoveBackward()
        {
            Console.WriteLine("Attempting to move backwards..");
            _vehicleBehaviour.MoveBackward();
        }

        public void MoveForward()
        {
            Console.WriteLine("Moving forwards..");
            _vehicleBehaviour.MoveForward();
        }

        public void Start()
        {
            Console.WriteLine("Starting engine..");
            _engine.TurnOn();
        }

        public void Stop()
        {
            Console.WriteLine("Stopping engine..");
            _engine.TurnOff();
        }
    }
}
