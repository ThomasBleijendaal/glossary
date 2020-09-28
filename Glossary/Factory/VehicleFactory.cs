using System;

namespace Factory
{
    public class VehicleFactory : IVehicleFactory
    {
        public IVehicle BuildVehicle(VehicleSpecifications spec)
        {
            return new Vehicle(GetEngine(spec.Engine), GetBehaviour(spec.Type));
        }

        private IVehicleBehaviour GetBehaviour(VehicleSpecifications.VehicleType type)
        {
            return type switch
            {
                VehicleSpecifications.VehicleType.Car => new CarBehaviour(),
                VehicleSpecifications.VehicleType.Plane => new PlaneBehaviour(),
                VehicleSpecifications.VehicleType.ToyCar => new ToyCarBehaviour(),
                _ => throw new InvalidOperationException()
            };
        }

        private IEngine GetEngine(VehicleSpecifications.EngineType type)
        {
            return type switch
            {
                VehicleSpecifications.EngineType.Electric => new ElectricEngine(),
                VehicleSpecifications.EngineType.Fake => new FakeEngine(),
                VehicleSpecifications.EngineType.Steam => new SteamEngine(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
