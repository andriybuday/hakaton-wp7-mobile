using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using PizzaDeliveryServices;

namespace ConsoleHostForPizzaDeliveryServices
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceHost = new ServiceHost(typeof(PizzaDelivery)))
            {
                serviceHost.Open();
                Console.WriteLine("Pizza Delivery Services up and running...");




                Console.WriteLine("Press any key to exit application...");
                Console.ReadKey();
            }
        }
    }
}
