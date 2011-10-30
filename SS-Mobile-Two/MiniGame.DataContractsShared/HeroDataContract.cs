using System.Runtime.Serialization;

namespace MiniGame.DataContractsShared
{
    [DataContract]
    public class HeroDataContact
    {
        [DataMember]
        public bool IsInYourTeam { get; set; }

        [DataMember]
        public byte[] MemberPhoto { get; set; }

        [DataMember]
        public string Name { get; set; }

        /*public Hero ToHero()
        {
            return new Hero()
                       {
                           IsInYourTeam = IsInYourTeam,
                           Name = Name,
                           MemberPhoto = ImageHelper.FromByteArray(MemberPhoto)
                       };
        }*/
    }
}
