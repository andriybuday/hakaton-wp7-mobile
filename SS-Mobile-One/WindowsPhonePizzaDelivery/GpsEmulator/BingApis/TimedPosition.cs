using System;
using System.Runtime.Serialization;
using System.Windows;
using GpsEmulator.MapControl;

namespace GpsEmulator.BingApis
{
    [DataContract]
    public class TimedPosition
    {
        [DataMember]
        public TimeSpan Time { get; set; }
        [DataMember]
        public Point Position { get; set; }
        public MapMarker MapMarker { get; set; }

        public TimedPosition(TimeSpan time, double lat, double lng)
        {
            this.Time = time;
            this.Position = new Point(lat, lng);
        }
    }
}
