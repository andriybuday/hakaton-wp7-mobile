using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WCEmergency.Common;

namespace WCEmergency.DataLayer
{
    public interface IDataFacade
    {
        IList<Toilet> GetNearestToiltes(Coordinate currrentPosition, double distance);
    }

    public class DataFacade
    {
        public IList<Common.Toilet> GetNearestToiltes(Coordinate currrentPosition, double distance)
        {
            var emergencyEntities = new WCEmergencyEntities();
            var q = from s in emergencyEntities.Toilets
                    select s;

            return new List<Common.Toilet>();
        }
    }
}
