using System;
using System.Collections.Generic;

namespace MiniGame.DataContractsShared
{
    public class Team
    {
        public Team()
        {
            Heros = new List<HeroDataContact>();
        }

        public IList<HeroDataContact> Heros { get; set; }

        public string Name { get; set; }

        public bool IsConfirmedStart { get; set; }

        public int BombCount { get; set; }

        public int MeCount { get; set; }

        public int EnemyCount { get; set; }

        public bool IsWinner { get; set; }

        public int BombsAdded { get; set; }

        public GameStateChanges LatestChanges { get; set; }
    }
}
