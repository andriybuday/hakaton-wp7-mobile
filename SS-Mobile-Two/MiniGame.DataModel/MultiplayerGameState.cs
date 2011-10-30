using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MiniGame.DataModel
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

        public Team GetTeamByName(string name)
        {
            if (Team1.Name == name)
                return Team1;
            if (Team2.Name == name)
                return Team2;
            return null;
        }

        public bool IsGameStarted { get; set; }

        public bool IsGameOver { get; set; }

        public bool IsReadyToStart
        {
            get
            {
                return !IsGameStarted && Team1 != null && Team2 != null && Team1.IsConfirmedStart &&
                       Team2.IsConfirmedStart;
            }
        }
    }
}
