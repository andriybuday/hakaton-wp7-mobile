using System.Runtime.Serialization;

namespace MiniGame.DataContractsShared
{
    [DataContract]
    public class GameStateChanges
    {
        [DataMember]
        public int BombsAdded { get; set; }

        [DataMember]
        public bool IsWinner { get; set; }

        [DataMember]
        public bool IsLoser { get; set; }
    }
}
