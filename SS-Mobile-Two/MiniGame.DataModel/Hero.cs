using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace MiniGame.DataModel
{
    
    public class Hero
    {
        public bool IsInYourTeam { get; set; }
        public WriteableBitmap MemberPhoto { get; set; }
        public string Name { get; set; }

        public HeroDataContact ToHeroDataContract()
        {
            return new HeroDataContact()
                       {
                           IsInYourTeam = IsInYourTeam,
                           Name = Name,
                           MemberPhoto = ImageHelper.ToByteArrayB(MemberPhoto)
                       };
        }
    }

    [DataContract]
    public class HeroDataContact
    {
        [DataMember]
        public bool IsInYourTeam { get; set; }

        [DataMember]
        public byte[] MemberPhoto { get; set; }

        [DataMember]
        public string Name { get; set; }

        public Hero ToHero()
        {
            return new Hero()
                       {
                           IsInYourTeam = IsInYourTeam,
                           Name = Name,
                           MemberPhoto = ImageHelper.FromByteArray(MemberPhoto)
                       };
        }
    }
}
