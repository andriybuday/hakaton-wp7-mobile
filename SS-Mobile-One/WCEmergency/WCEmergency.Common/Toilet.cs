using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCEmergency.Common
{
    [DataContract]
    public enum Sex:int
    { 
        Male = 1,
        Female = 2 ,
        Unisex = 3
    }

    [DataContract]
    public class Toilet
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int? Rate { get; set; }

        [DataMember]
        public Coordinate Coordinate { get; set; }

        [DataMember]
        public byte[] Picture { get; set; }

        [DataMember]
        public Sex Sex { set; get; }
    }
}
