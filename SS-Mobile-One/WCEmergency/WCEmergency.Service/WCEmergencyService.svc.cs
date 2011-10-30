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
                                   Coordinate = new GeoCoordinate(){Latitude = 49.81545, Longitude = 24.00187, Altitude = 0},
                                   Sex = Sex.Unisex, 	
                                   Rate = 10//,
                                  // Picture = LoadFromFile(@"D:\_Data\V.png")
                               },
                               new Toilet()
                               {
                                   Id = 2,
                                   Name = "McDonalds, st. V.Velukogo, 24а.",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.81179, Longitude = 23.98642, Altitude = 0},
                                   Sex = Sex.Unisex,//	
                                   Rate = 10//,
                                  // Picture = LoadFromFile(@"D:\_Data\V.png")
                               },
                               new Toilet()
                               {
                                   Id = 3,
                                   Name = "Toilet in centre",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.842065, Longitude = 24.028527, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 10//,
                                 //  Picture = LoadFromFile(@"D:\_Data\V.png")
                               },
                                 

                           new Toilet()
                               {
                                   Id = 6,
                                   Name = "Technical building",
                                   Description = "At the 1-st floor",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.82968, Longitude = 23.99028, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 3//,
                                  // Picture = LoadFromFile(@"D:\_Data\Kultp1.png")
                               },

                           new Toilet()
                               {
                                   Id = 7,
                                   Name = "Railway station",
                                   Description = "At the 1-st floor",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.83956, Longitude = 		23.99508, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                  // Picture = LoadFromFile(@"D:\_Data\Vokzal.png")
                               },

                           new Toilet()
                               {
                                   Id = 8,
                                   Name = "Railway station",
                                   Description = "At the 1-st floor",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.83956, Longitude = 		23.99508, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                  // Picture = LoadFromFile(@"D:\_Data\Vokzal.png")
                               },
                   new Toilet()
                               {
                                   Id = 9,
                                   Name = "Politechnic University",
                                   Description = "At the 2-st floor, go to the right",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.83956, Longitude = 		23.99508, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                //   Picture = LoadFromFile(@"D:\_Data\Vokzal.png")
                               },
                   new Toilet()
                               {
                                   Id = 10,
                                   Name = "Cafe",
                                   Description = "At the 1-st floor, go to the left",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.83491, Longitude = 		24.00191, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                  // Picture = LoadFromFile(@"D:\_Data\Vokzal.png")
                               },
                   new Toilet()
                               {
                                   Id = 11,
                                   Name = "Politechnic University",
                                   Description = "At the 1-st floor, go to the left",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.83472, Longitude = 		24.01517, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                  // Picture = LoadFromFile(@"D:\_Data\Polythehnic.png")
                               },

                                            
new Toilet()
                               {
                                   Id = 1,
                                   Name = "Cafe",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 44.71145, Longitude = 21.13487, Altitude = 0},
                                   Sex = Sex.Unisex, 	
                                   Rate = 7,
                                   Picture = null
                               },              
             new Toilet()
                               {
                                   Id = 1,
                                   Name = "Cafe",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.71145, Longitude = 24.13187, Altitude = 0},
                                   Sex = Sex.Unisex, 	
                                   Rate = 7,
                                   Picture = null
                               },              
             new Toilet()
                               {
                                   Id = 1,
                                   Name = "Cafe",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.81145, Longitude = 24.02187, Altitude = 0},
                                   Sex = Sex.Unisex, 	
                                   Rate = 7,
                                   Picture = null
                               },
                         new Toilet()
                               {
                                   Id = 1,
                                   Name = "Cafe",
                                   Description = "Good, nice and free toilet for McDonalds visitors",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.81001, Longitude = 23.02187, Altitude = 0},
                                   Sex = Sex.Unisex, 	
                                   Rate = 3,
                                   Picture = null
                               },


                               new Toilet()
                               {
                                   Id = 3,
                                   Name = "Toilet in park",
                                   Description = "Awful",
                                   Coordinate = new GeoCoordinate(){Latitude = 41.812065, Longitude = 21.028527, Altitude = 0},
                                   Sex = Sex.Male,
                                   Rate = 2,
                                   Picture = null
                               },



                               new Toilet()
                               {
                                   Id = 3,
                                   Name = "cafe",
                                   Description = "Great",
                                   Coordinate = new GeoCoordinate(){Latitude = 50.112065, Longitude = 22.028527, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 2,
                                   Picture = null
                               },
                                 

                           new Toilet()
                               {
                                   Id = 6,
                                   Name = "Technical building",
                                   Description = "At the 1-st floor",
                                   Coordinate = new GeoCoordinate(){Latitude = 50.01968, Longitude = 24.00028, Altitude = 0},
                                   Sex = Sex.Male,
                                   Rate = 3,
                                   Picture = null
                               },

                           new Toilet()
                               {
                                   Id = 7,
                                   Name = "On the street",
                                   Description = "Ж))",
                                   Coordinate = new GeoCoordinate(){Latitude = 50.23956, Longitude = 		23.99508, Altitude = 0},
                                   Sex = Sex.Male,
                                   Rate = 5,
                                   Picture = null
                               },

                           new Toilet()
                               {
                                   Id = 8,
                                   Name = "Cafe",
                                   Description = "At the 1-st floor",
                                   Coordinate = new GeoCoordinate(){Latitude = 48.83956, Longitude = 		22.29508, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                   Picture = null
                               },
                   new Toilet()
                               {
                                   Id = 9,
                                   Name = "In University",
                                   Description = "At the 1-st floor, go to the right",
                                   Coordinate = new GeoCoordinate(){Latitude = 47.83956, Longitude = 		23.49508, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                   Picture = null
                               },
                   new Toilet()
                               {
                                   Id = 10,
                                   Name = "Petrol station",
                                   Description = "Man only",
                                   Coordinate = new GeoCoordinate(){Latitude = 50.82491, Longitude = 		21.00191, Altitude = 0},
                                   Sex = Sex.Male,
                                   Rate = 5,
                                   Picture = null
                               },
                   new Toilet()
                               {
                                   Id = 11,
                                   Name = "Politechnic University",
                                   Description = "At the 1-st floor, go to the left",
                                   Coordinate = new GeoCoordinate(){Latitude = 49.88872, Longitude = 		25.01517, Altitude = 0},
                                   Sex = Sex.Unisex,
                                   Rate = 5,
                                   Picture = null
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
