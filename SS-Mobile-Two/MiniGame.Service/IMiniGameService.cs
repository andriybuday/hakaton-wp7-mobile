using System.Collections.Generic;
using System.ServiceModel;
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
        Team GetEnemyTeam(string myTeamName);

        [OperationContract]
        GameStateChanges AddBombToEnemy(int count, string myTeamName);

        [OperationContract]
        GameStateChanges InformAboutWin(string myTeamName);

        [OperationContract]
        GameStateChanges InformAboutLose(string myTeamName);

        [OperationContract]
        GameStateChanges RetrieveChanges(string myTeamName);

    }
}
