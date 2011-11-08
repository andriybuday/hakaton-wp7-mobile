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
                    return _state.Team2;
                }
                
                if (myTeamName == _state.Team2.Name)
                {
                    _state.Team2Ready = true;
                    return _state.Team1;
                }

                return null;
            }            
        }

        public void ClearGame()
        {
            lock (_lockObject)
            {
                Team team1 = _state.Team1;
                Team team2 = _state.Team2;

                team1.IsWinner = false;
                team1.IsLoser = false;
                team1.BombsAdded = 0;

                team2.IsWinner = false;
                team2.IsLoser = false;
                team2.BombsAdded = 0;
            }
        }


        public GameStateChanges AddBombToEnemy(int count, string myTeamName)
        {
            lock (_lockObject)
            {
                Team otherTeam = _state.GetOtherTeamByName(myTeamName);
                otherTeam.BombsAdded += count;

                return ReturnCurrentState(myTeamName);
            }
        }

        private GameStateChanges ReturnCurrentState(string teamName)
        {
            Team myTeam = _state.GetTeamByName(teamName);

            var changesToReturn = new GameStateChanges()
            {
                BombsAdded = myTeam.BombsAdded,
                IsLoser = myTeam.IsLoser,
                IsWinner = myTeam.IsWinner
            };

            myTeam.BombsAdded = 0;
            if (myTeam.IsLoser || myTeam.IsWinner)
            {
                ClearGame();
            }
            return changesToReturn;
        }


        public GameStateChanges InformAboutWin(string myTeamName)
        {
            lock (_lockObject)
            {
                Team myTeam = _state.GetTeamByName(myTeamName);
                Team otherTeam = _state.GetOtherTeamByName(myTeamName);

                if (!myTeam.IsWinner && !myTeam.IsLoser)
                {
                    myTeam.IsWinner = true;
                    otherTeam.IsLoser = true;
                }

                return ReturnCurrentState(myTeamName);
            }
        }

        public GameStateChanges InformAboutLose(string myTeamName)
        {
            lock (_lockObject)
            {
                Team myTeam = _state.GetTeamByName(myTeamName);
                Team otherTeam = _state.GetOtherTeamByName(myTeamName);

                if (!myTeam.IsWinner && !myTeam.IsLoser)
                {
                    myTeam.IsLoser = true;
                    otherTeam.IsWinner = true;
                }

                return ReturnCurrentState(myTeamName);
            }
        }


        public GameStateChanges RetrieveChanges(string myTeamName)
        {
            lock (_lockObject)
            {
                return ReturnCurrentState(myTeamName);
            }
        }
    }
}
