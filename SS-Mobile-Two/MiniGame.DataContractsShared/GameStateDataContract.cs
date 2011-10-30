using System;
using System.Runtime.Serialization;

namespace MiniGame.DataContractsShared
{
    [DataContract]
    public class GameStateDataContact
    {
        [DataMember]
        public int Seconds { get; set; }

        [DataMember]
        public DateTime GameStartTime { get; set; }

        [DataMember]
        public bool IsVinner {get; set; }

        [DataMember]
        public bool IsGameOver { get; set; }

        [DataMember]
        public int BombCount { get; set;}

        [DataMember]
        public int MeCount { get; set; }

        [DataMember]
        public int EnemyCount { get; set; }

        [DataMember]
        public string TeamName { get; set; }
    }
}
