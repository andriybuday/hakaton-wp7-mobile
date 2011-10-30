using System;
using System.Runtime.Serialization;

namespace MiniGame.DataContractsShared
{
    [DataContract]
    public class GameStateChanges
    {
        [DataMember]
        public int BombsAdded { get; set; }

        [DataMember]
        public int BombsRemoved { get; set; }

        [DataMember]
        public int FriendsRemoved {get; set; }

        [DataMember]
        public int EnemiesRemoved { get; set; }

        [DataMember]
        public string TeamName { get; set; }

        [DataMember]
        public bool IsWinner { get; set; }

        [DataMember]
        public bool IsGameOver { get; set; }
    }
}
