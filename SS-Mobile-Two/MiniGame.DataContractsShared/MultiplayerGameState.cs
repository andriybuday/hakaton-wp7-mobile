using System;

namespace MiniGame.DataContractsShared
{
    public class MultiplayerGameState
    {
        private static MultiplayerGameState _instance;
        private MultiplayerGameState()
        {
        }

        public static MultiplayerGameState GetInstance()
        {
            return _instance ?? (_instance = new MultiplayerGameState()
                                                 {
                                                     Team1 = new Team(),
                                                     Team2 = new Team()
                                                 });
        }

        public Team Team1 { get; set; }

        public Team Team2 { get; set; }

        public DateTime StartTime { get; set; }

        public bool Team1Ready { get; set; }

        public bool Team2Ready { get; set; }

        public bool BothTeamReady
        {
            get { return Team1Ready && Team2Ready; }
        }

        public Team GetTeamByName(string name)
        {
            if (Team1.Name == name)
                return Team1;
            if (Team2.Name == name)
                return Team2;
            return null;
        }

        public Team GetOtherTeamByName(string name)
        {
            if (Team1.Name == name)
                return Team2;
            if (Team2.Name == name)
                return Team1;
            return null;
        }

        public bool IsGameStarted { get; set; }

        public bool IsGameOver { get; set; }

    }
}
