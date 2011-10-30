using System.Runtime.Serialization;

namespace MiniGame.DataContractsShared
{
    [DataContract]
    public class GameStateDataContact
    {
        [DataMember]
        public bool IsVinner {get; set; }

        [DataMember]
        public bool GameStarted { get; set; }

        [DataMember]
        public bool GameOver { get; set; }

        [DataMember]
        public bool BombCount { get; set; }

    }
}
