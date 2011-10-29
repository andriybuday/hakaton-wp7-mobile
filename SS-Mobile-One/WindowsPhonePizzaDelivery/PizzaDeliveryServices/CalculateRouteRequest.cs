using System.Runtime.Serialization;

namespace PizzaDeliveryServices
{
    [DataContract]
    public class CalculateRouteRequest
    {
        [DataMember]
        public string StartAddress { get; set; }

        ///<summary>
        ///</summary>
        [DataMember]
        public string EndAddress { get; set; }

        ///<summary>
        ///</summary>
        [DataMember]
        public double? StartLatitude { get; set; }

        ///<summary>
        ///</summary>
        [DataMember]
        public double? StartLongitude { get; set; }

        ///<summary>
        ///</summary>
        [DataMember]
        public double? EndLatitude { get; set; }

        ///<summary>
        ///</summary>
        [DataMember]
        public double? EndLongitude { get; set; }
    }
}