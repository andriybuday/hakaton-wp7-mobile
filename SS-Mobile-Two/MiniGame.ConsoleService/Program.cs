using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using MiniGame.Service;

namespace MiniGame.ConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                  var serviceHost = new ServiceHost(typeof(MiniGameService), new Uri("http://localhost:9991/MiniGameService"));

                  serviceHost.Open();
                  Console.WriteLine("SERVER - Running...");
                  Console.ReadKey();

                  serviceHost.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
