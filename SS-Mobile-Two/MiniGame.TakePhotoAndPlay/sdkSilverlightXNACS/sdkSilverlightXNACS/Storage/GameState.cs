using System.Collections.Generic;
using System.Windows.Media.Imaging;
using sdkSilverlightXNACS.Models;

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
            return _instance ?? (_instance = new GameState() { MyTeam = new List<Hero>(), EnemyTeam = new List<Hero>()});
        }

        public bool IsGameStarted { get; set; }

        public IList<Hero> MyTeam { get; set; }

        public IList<Hero> EnemyTeam { get; set; }

        public WriteableBitmap Background { get; set; }
    }
}
