using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCEmergency.Common;
using WCEmergency.DataLayer;
using Toilet = WCEmergency.Common.Toilet;

namespace WCEmergency.Service
{
    public class WCEmergencyService : IWCEmergencyService
    {

        public IList<Common.Toilet> GetNearestToiltes(Coordinate currrentPosition, double distance)
        {
            DataFacade dataFacade = new DataFacade();
            return dataFacade.GetNearestToiltes(currrentPosition, distance);
        }
    }
}
