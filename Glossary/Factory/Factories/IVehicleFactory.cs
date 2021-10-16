using Factory.Vehicles;

namespace Factory.Factories
{
    public interface IVehicleFactory
    {
        IVehicle BuildVehicle(VehicleSpecifications spec);
    }
}
