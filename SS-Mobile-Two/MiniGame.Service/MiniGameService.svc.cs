using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MiniGame.Service
{
    public class MiniGameService : IMiniGameService
    {

        public string GetState(string currrentPosition)
        {
            return "OK";
        }

        public bool Start(string player)
        {
            return true;
        }
    }
}
