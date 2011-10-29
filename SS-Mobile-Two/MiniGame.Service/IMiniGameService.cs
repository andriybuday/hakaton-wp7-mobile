using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace MiniGame.Service
{
    [ServiceContract]
    public interface IMiniGameService
    {
        [OperationContract]
        string GetState(string currrentPosition);

        [OperationContract]
        bool Start(string player);
    }

}
