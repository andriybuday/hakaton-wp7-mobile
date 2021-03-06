﻿using System.Device.Location;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCEmergency.Common
{
    [DataContract]
    public enum Sex:int
    {
        [EnumMember]
        Male = 1,
        [EnumMember]
        Female = 2 ,
        [EnumMember]
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
        public GeoCoordinate Coordinate { get; set; }

        [DataMember]
        public byte[] Picture { get; set; }

        [DataMember]
        public Sex Sex { set; get; }
    }
}
