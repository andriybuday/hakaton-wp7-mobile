using System.Collections.Generic;
using System.Windows.Media.Imaging;
using MiniGame.DataModel;

namespace sdkSilverlightXNACS.Storage
{
    public class GameState
    {
        private static GameState _instance;
        private GameState()
        {
        }

        public static GameState GetInstance()
        {
            return _instance ?? (_instance = new GameState() { FriendsTeam = new List<Hero>(), EnemyTeam = new List<Hero>() });
        }

        public bool IsGameStarted { get; set; }

        public bool? IsMultiPlayerGame { get; set; }

        public bool IsGameOver { get; set; }

        public IList<Hero> FriendsTeam { get; set; }

        public IList<Hero> EnemyTeam { get; set; }

        public WriteableBitmap Background { get; set; }

        public bool AmIReady { get; set; }

        public bool IsEnemyReady { get; set; }

        public int Player1WonInSeconds { get; set; }

        public int Player2WonInSeconds { get; set; }
    }
}
