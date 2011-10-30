using System;
using System.Collections.Generic;
using MiniGame.DataContractsShared;

namespace MiniGame.Service
{
    public class MiniGameService : IMiniGameService
    {
        private MultiplayerGameState _state = MultiplayerGameState.GetInstance();
        private object _lockObject = new object();

        public string RegisterMe()
        {
            lock (_lockObject)
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
            lock (_lockObject)
            {
                foreach (HeroDataContact hero in myHeros)
                {
                    _state.GetTeamByName(myName).Heros.Add(hero);
                }
                return true;
            }
        }

        public Team GetEnemyTeam(string myTeamName)
        {
            lock (_lockObject)
            {
                if (myTeamName == _state.Team1.Name)
                {
                    _state.Team1Ready = true;
                    //SetStartTime();
                    return _state.Team2;
                }
                
                if (myTeamName == _state.Team2.Name)
                {
                    _state.Team2Ready = true;
                    //SetStartTime();
                    return _state.Team1;
                }

                return null;
            }            
        }

        private void SetStartTime()
        {
            lock (_state)
            {
                if (_state.BothTeamReady)
                {
                    //RestartGame(_state.Team1.Name);
                }
            }
        }


        public GameStateChanges GetMyInfo(GameStateChanges myTeamInfo)
        {
            lock (_lockObject)
            {
                Team myTeam = _state.GetTeamByName(myTeamInfo.TeamName);

                Team otherTeam = _state.GetOtherTeamByName(myTeamInfo.TeamName);

                otherTeam.BombsAdded += myTeamInfo.EnemiesRemoved;
                otherTeam.LatestChanges = new GameStateChanges() {BombsAdded = myTeamInfo.EnemiesRemoved};
                myTeamInfo.EnemiesRemoved = 0;

                var latestChanges = new GameStateChanges() { BombsAdded = myTeam.BombsAdded };

                if (myTeamInfo.IsGameOver || _state.IsGameOver)
                {
                    latestChanges.IsGameOver = true;
                    if (_state.GetOtherTeamByName(myTeamInfo.TeamName).IsWinner)
                    {
                        latestChanges.IsWinner = false;
                    }
                    else if (myTeamInfo.IsWinner)
                    {
                        latestChanges.IsWinner = true;
                        myTeam.IsWinner = true;
                    }
                    else
                    {
                        latestChanges.IsWinner = false;
                        myTeam.IsWinner = false;
                    }
                }
            
                return latestChanges;
            }
        }

        public void RestartGame(string myName)
        {
            lock (_lockObject)
            {
                Team myTeam = _state.GetTeamByName(myName);
                Team otherTeam = _state.GetOtherTeamByName(myName);

                myTeam.IsWinner = false;
                myTeam.BombsAdded = 0;

                otherTeam.IsWinner = false;
                otherTeam.BombsAdded = 0;

                _state.StartTime = DateTime.Now;
            }
        }
    }
}
