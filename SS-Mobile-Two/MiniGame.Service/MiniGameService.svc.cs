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
    public class MiniGameService : IMiniGameService
    {
        private MultiplayerGameState _state = MultiplayerGameState.GetInstance();

        public string RegisterMe()
        {
            lock (_state)
            {
                if (string.IsNullOrEmpty(_state.Team1.Name))
                {
                    _state.Team1.Name = Guid.NewGuid().ToString();
                    return _state.Team1.Name;
                }
                if (string.IsNullOrEmpty(_state.Team2.Name))
                {
                    _state.Team2.Name = Guid.NewGuid().ToString();
                    return _state.Team2.Name;
                }
            }
            return _state.Team1.Name;
        }

        public bool SetTeam(string myName, IList<HeroDataContact> myHeros)
        {
            foreach(HeroDataContact hero in myHeros)
            {
                _state.GetTeamByName(myName).Heros.Add(hero);
            }
            return true;
        }

        public IList<HeroDataContact> GetEnemyTeam(string myTeamName)
        {
            if (myTeamName == _state.Team1.Name)
                return _state.Team2.Heros;
            if (myTeamName == _state.Team2.Name)
                return _state.Team1.Heros;
            return null;
        }
    }
}
