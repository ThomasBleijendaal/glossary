using System;

namespace Factory.Behaviours
{
    public class PlaneBehaviour : IVehicleBehaviour
    {
        public void MoveBackward() => Console.WriteLine("*Crashes*");

        public void MoveForward() => Console.WriteLine("*Whoosh*");
    }
}
