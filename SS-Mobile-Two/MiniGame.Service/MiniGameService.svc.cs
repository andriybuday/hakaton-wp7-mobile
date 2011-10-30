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
            lock (_state)
            {
                foreach (HeroDataContact hero in myHeros)
                {
                    _state.GetTeamByName(myName).Heros.Add(hero);
                }
                return true;
            }
        }

        public IList<HeroDataContact> GetEnemyTeam(string myTeamName)
        {
            lock (_state)
            {
                if (myTeamName == _state.Team1.Name)
                {
                    _state.Team1Ready = true;
                    SetStartTime();
                    return _state.Team2.Heros;
                }
                
                if (myTeamName == _state.Team2.Name)
                {
                    _state.Team2Ready = true;
                    SetStartTime();
                    return _state.Team1.Heros;
                }

                return null;
            }            
        }

        private void SetStartTime()
        {
            if (_state.BothTeamReady)
                _state.StartTime = DateTime.Now;
        }


        public GameStateDataContact GetMyInfo(GameStateDataContact myTeamInfo)
        {
            Team myTeam = _state.GetTeamByName(myTeamInfo.TeamName);

            myTeam.BombCount = myTeamInfo.BombCount;
            myTeam.EnemyCount = myTeamInfo.EnemyCount;
            myTeam.MeCount= myTeamInfo.MeCount;

            
        }
    }
}
