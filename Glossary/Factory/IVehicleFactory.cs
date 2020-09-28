namespace Factory
{
    public interface IVehicleFactory
    {
        IVehicle BuildVehicle(VehicleSpecifications spec);
    }
}
