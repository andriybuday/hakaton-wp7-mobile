using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PizzaDeliveryServices
{
    [ServiceContract]
    public interface IPizzaDelivery
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        CalculateMileageResult CalculateMileage(CalculateMileageRequest request);

        [OperationContract]
        CalculateRouteResult CalculateRoute(CalculateRouteRequest request);
    }
}
