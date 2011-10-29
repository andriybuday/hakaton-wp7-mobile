using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PizzaDeliveryServices
{
    [DataContract]
    public class CalculateRouteResult
    {
        public CalculateRouteResult()
        {
        }

        public CalculateRouteResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public CalculateRouteResult(bool success, string message, IList<RouteDirectionModel> directions)
        {
            Success = success;
            Message = message;
            Directions = directions;
        }

        [DataMember]
        public bool Success { get; private set; }
        [DataMember]
        public string Message { get; private set; }
        [DataMember]
        public IList<RouteDirectionModel> Directions { get; set; }
    }
}