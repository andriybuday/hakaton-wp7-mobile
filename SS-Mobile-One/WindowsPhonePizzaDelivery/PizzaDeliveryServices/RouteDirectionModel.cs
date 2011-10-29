using System;
using System.Runtime.Serialization;

namespace PizzaDeliveryServices
{
    [DataContract]
    public class RouteDirectionModel
    {
        public RouteDirectionModel(string description, int index, double mileage)
        {
            Description = description;
            Index = index;
            Mileage = mileage;
        }

        [DataMember(Name = "Index")]
        public Int32 Index { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        [DataMember(Name = "Mileage")]
        public double Mileage { get; set; }
    }
}