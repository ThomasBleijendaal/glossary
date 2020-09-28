﻿using System;

namespace Factory
{
    public class ToyCarBehaviour : IVehicleBehaviour
    {
        public void MoveBackward() => Console.WriteLine("*Squeek squeek*");

        public void MoveForward() => Console.WriteLine("*Squeek squeek*");
    }
}
