using System;

namespace Factory.Behaviours
{
    public class CarBehaviour : IVehicleBehaviour
    {
        public void MoveBackward() => Console.WriteLine("*Beep beep beep*");

        public void MoveForward() => Console.WriteLine("*Vrooooom*");
    }
}
