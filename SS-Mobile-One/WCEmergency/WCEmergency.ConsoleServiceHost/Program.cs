using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using WCEmergency.Service;

namespace WCEmergency.ConsoleServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the WCF Console Host");

            var serviceHost = new ServiceHost(typeof(WCEmergencyService), new Uri("http://localhost:9998/WCEmergency"));

            serviceHost.Open();

            Console.ReadKey();

            serviceHost.Close();
        }
    }
}
