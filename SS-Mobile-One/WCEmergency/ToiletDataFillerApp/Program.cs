using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using WCEmergency.DataLayer;
using WCEmergency.Common;

namespace ToiletDataFillerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string _FileName = @"D:\VV.png";
            byte[] _Buffer = null;

   try
	    {
	        // Open file for reading
	        System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
	         
	        // attach filestream to binary reader
	        System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);
	         
	        // get total byte length of the file
	        long _TotalBytes = new System.IO.FileInfo(_FileName).Length;
	         
	        // read entire file into buffer
	        _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);
	         
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
            
            
            DataFacade facade = new DataFacade();
            facade.AddToilet(new WCEmergency.Common.Toilet() {
                Coordinate = new GeoCoordinate() { Latitude = 49.839683, Longitude = 24.029717 },
                Description = "МакДональдс / швидка їжа з усіма вигодами і наслідками:)",
                Name = "пр. Чорновола, 12.",
                Rate = 10,
                Sex = Sex.Unisex,
                Picture = _Buffer
                 });

        }
    }
}
