using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MiniGame.DataContractsShared;


namespace MiniGame.Service
{
    [ServiceContract]
    public interface IMiniGameService
    {
        [OperationContract]
        string RegisterMe();

        [OperationContract]
        bool SetTeam(string myName, IList<HeroDataContact> myHeros);

        [OperationContract]
        IList<HeroDataContact> GetEnemyTeam(string myTeamName);
    }

}
