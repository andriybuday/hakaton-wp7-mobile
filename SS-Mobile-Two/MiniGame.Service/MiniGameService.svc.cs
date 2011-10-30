using System;
using System.Collections.Generic;
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

        public Team GetEnemyTeam(string myTeamName)
        {
            lock (_state)
            {
                if (myTeamName == _state.Team1.Name)
                {
                    _state.Team1Ready = true;
                    SetStartTime();
                    return _state.Team2;
                }
                
                if (myTeamName == _state.Team2.Name)
                {
                    _state.Team2Ready = true;
                    SetStartTime();
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
                    RestartGame(_state.Team1.Name);
                }
            }
        }


        public GameStateChanges GetMyInfo(GameStateChanges myTeamInfo)
        {
            lock (_state)
            {
                Team myTeam = _state.GetTeamByName(myTeamInfo.TeamName);

                myTeam.BombCount -= myTeamInfo.BombsRemoved;
                myTeam.EnemyCount -= myTeamInfo.EnemiesRemoved;
                myTeam.MeCount -= myTeamInfo.FriendsRemoved;

                Team otherTeam = GetEnemyTeam(myTeamInfo.TeamName);

                otherTeam.BombCount += myTeamInfo.EnemiesRemoved;
                otherTeam.LatestChanges = new GameStateChanges() {BombsAdded = myTeamInfo.EnemiesRemoved};

                var latestChanges = myTeam.LatestChanges;
                myTeam.LatestChanges = new GameStateChanges();

                if (myTeamInfo.IsGameOver || _state.IsGameOver)
                {
                    latestChanges.IsGameOver = true;
                    if (GetEnemyTeam(myTeamInfo.TeamName).IsWinner)
                    {
                        latestChanges.IsWinner = false;
                    }
                    else
                    {
                        latestChanges.IsWinner = true;
                        myTeam.IsWinner = true;
                    }
                }
            
                return latestChanges;
            }
        }

        public void RestartGame(string myName)
        {
            lock (_state)
            {
                Team myTeam = _state.GetTeamByName(myName);
                Team otherTeam = _state.GetOtherTeamByName(myName);

                myTeam.IsWinner = false;
                myTeam.BombCount = 1;
                myTeam.EnemyCount = otherTeam.Heros.Count;
                myTeam.MeCount = myTeam.Heros.Count;
                myTeam.LatestChanges = new GameStateChanges();

                otherTeam.IsWinner = false;
                otherTeam.BombCount = 1;
                otherTeam.EnemyCount = myTeam.Heros.Count;
                otherTeam.MeCount = otherTeam.Heros.Count;
                otherTeam.LatestChanges = new GameStateChanges();

                _state.StartTime = DateTime.Now;
            }
        }
    }
}
