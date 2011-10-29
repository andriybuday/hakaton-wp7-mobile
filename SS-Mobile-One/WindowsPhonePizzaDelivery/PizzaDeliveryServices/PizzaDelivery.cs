using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PizzaDeliveryServices
{
    public class PizzaDelivery : IPizzaDelivery
    {
        BingMapsServices _bingMapsServices = new BingMapsServices();

        public string Ping()
        {
            return string.Format(@"I'm alive from Pizza Delivery Service...");
        }

        public CalculateMileageResult CalculateMileage(CalculateMileageRequest request)
        {
            return _bingMapsServices.CalculateMileage(request);
        }

        public CalculateRouteResult CalculateRoute(CalculateRouteRequest request)
        {
            return _bingMapsServices.CalculateRoute(request);
        }
    }
}
