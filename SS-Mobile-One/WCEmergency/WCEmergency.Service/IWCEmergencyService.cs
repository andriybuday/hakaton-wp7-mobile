using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WCEmergency.Common;

namespace WCEmergency.Service
{
    [ServiceContract]
    public interface IWCEmergencyService
    {
        [OperationContract]
        IList<Common.Toilet> GetNearestToiltes(Coordinate currrentPosition, double distance);

        [OperationContract]
        void AddToilet(Toilet newToilet);
    }

}
