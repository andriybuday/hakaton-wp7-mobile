using System.Net.Mime;

namespace WCEmergency.Common
{
    public enum Sex:int
    { 
        Male = 1,
        Female = 2 ,
        Unisex = 3
    }

    public class Toilet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Rate { get; set; }
        public Coordinate Coordinate { get; set; }
        public byte[] Picture { get; set; }
        public Sex Sex { set; get; }
    }
}
