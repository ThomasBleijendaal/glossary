/**
* A factory is a class that resolves a freshly created object which implements a certain interface
* that suits the request made to the factory. It can leverage a IServiceProvider to compose the object.
* 
* It differs from a Resolver or Provider in that it returns an object which the context will use,
* and performs actions with, instead of just resolving a piece of information or providing a strategy.
* 
* A factory can become substantial but should focus only on creating objects.
*/

using System;
using System.Threading.Tasks;
using Factory.Factories;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace Factory
{
    public class Program : BaseProgram
    {
        public static async Task Main(string[] args)
        {
            await Init<Program, FactoryApp>();
        }

        protected override void Startup(ServiceCollection services)
        {
            services.AddSingleton<IVehicleFactory, VehicleFactory>();
        }

        public class FactoryApp : BaseApp
        {
            private readonly IVehicleFactory _vehicleFactory;

            public FactoryApp(IVehicleFactory vehicleFactory)
            {
                _vehicleFactory = vehicleFactory;
            }

            public override Task Run()
            {
                do
                {
                    var spec = new VehicleSpecifications
                    {
                        Engine = (VehicleSpecifications.EngineType)new Random().Next(1, 4),
                        Type = (VehicleSpecifications.VehicleType)new Random().Next(1, 4)
                    };

                    var vehicle = _vehicleFactory.BuildVehicle(spec);

                    vehicle.Start();
                    vehicle.MoveForward();
                    vehicle.MoveBackward();
                    vehicle.Stop();

                    Console.WriteLine("Press enter to make another vehicle.");
                    Console.ReadLine();
                }
                while (true);
            }
        }
    }
}
