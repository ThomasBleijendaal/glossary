namespace Factory
{
    public class VehicleSpecifications
    {
        public EngineType Engine { get; set; }

        public VehicleType Type { get; set; }

        public enum EngineType
        {
            Fake = 1,
            Electric = 2,
            Steam = 3
        }

        public enum VehicleType
        {
            ToyCar = 1,
            Plane = 2,
            Car = 3
        }
    }
}
