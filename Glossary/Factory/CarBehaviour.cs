using System;

namespace Factory
{
    public class CarBehaviour : IVehicleBehaviour
    {
        public void MoveBackward() => Console.WriteLine("*Beep beep beep*");

        public void MoveForward() => Console.WriteLine("*Vrooooom*");
    }
}
