using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCEmergency.Common;
using Toilet = WCEmergency.Common.Toilet;

namespace WCEmergency.Service
{
    public class WCEmergencyService : IWCEmergencyService
    {
        private byte[] LoadFromFile(string filePath)
        {
            byte[] _Buffer = null;
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                                                                            System.IO.FileAccess.Read);

                // attach filestream to binary reader
                System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);

                // get total byte length of the file
                long _TotalBytes = new System.IO.FileInfo(filePath).Length;

                // read entire file into buffer
                _Buffer = _BinaryReader.ReadBytes((Int32) _TotalBytes);

                // close file reader
                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            return _Buffer;

        }

        
        private IList<Common.Toilet> GetAllhardcodedToiltes()
        {
            return new List<Toilet>()
                       {
                           new Toilet()
                               {
                                   Id = 1,
                                   Name = "McDonalds, st. V.Velukogo, 24а.",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.839683, Longitude = 24.029717, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 10,
                                   Picture = LoadFromFile(@"D:\_Data\V.png")
                               },
                               new Toilet()
                               {
                                   Id = 2,
                                   Name = "McDonalds, st. V.Velukogo, 24а.",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.839683, Longitude = 24.029717, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 10,
                                   Picture = LoadFromFile(@"D:\_Data\V.png")
                               },
                               new Toilet()
                               {
                                   Id = 3,
                                   Name = "Toilet in centre",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.842065, Longitude = 24.028527, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 10,
                                   Picture = LoadFromFile(@"D:\_Data\V.png")
                               }
                       };

        }

        public IList<Common.Toilet> GetNearestToiltes(GeoCoordinate currrentPosition, double distance)
        {
            var q = from s in GetAllhardcodedToiltes()
                    orderby Math.Pow(currrentPosition.Latitude - s.Coordinate.Latitude, 2) + Math.Pow(currrentPosition.Longitude - s.Coordinate.Longitude, 2) 
                    select s ;
            return q.ToList();

            /*
            DataFacade dataFacade = new DataFacade();
            return dataFacade.GetNearestToiltes(currrentPosition, distance);
             */
        }

        public void AddToilet(Toilet newToilet)
        {
            //Do nothing
            /*
            DataFacade dataFacade = new DataFacade();
            dataFacade.AddToilet(newToilet);
             */
        }
    }
}
