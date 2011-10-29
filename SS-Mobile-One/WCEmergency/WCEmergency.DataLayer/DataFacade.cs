using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using WCEmergency.Common;

namespace WCEmergency.DataLayer
{
    public interface IDataFacade
    {
        IList<Common.Toilet> GetNearestToiltes(GeoCoordinate currrentPosition, double distance);

        void AddToilet(Common.Toilet newToilet);
    }

    public class DataFacade : IDataFacade
    {
        public IList<Common.Toilet> GetNearestToiltes(GeoCoordinate currrentPosition, double distance)
        {
            var emergencyEntities = new WCEmergencyEntities();
            var q = from s in emergencyEntities.Toilets
                    orderby Math.Pow(currrentPosition.Latitude - s.CoordinateX, 2) + Math.Pow(currrentPosition.Longitude - s.CoordinateY, 2) 
                    select s ;


            return ToiletMapper.Map(q.ToList());
        }

        public void AddToilet(Common.Toilet newToilet)
        {
            var emergencyEntities = new WCEmergencyEntities();
            emergencyEntities.Toilets.AddObject(ToiletMapper.Map(newToilet));
            emergencyEntities.SaveChanges();
        }

    }
}
