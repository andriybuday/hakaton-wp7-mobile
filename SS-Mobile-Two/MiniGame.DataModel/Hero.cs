using System.Windows.Media.Imaging;

namespace MiniGame.DataModel
{
    
    public class Hero
    {
        public bool IsInYourTeam { get; set; }
        public WriteableBitmap MemberPhoto { get; set; }
        public string Name { get; set; }

        /*public HeroDataContact ToHeroDataContract()
        {
            return new HeroDataContact()
                       {
                           IsInYourTeam = IsInYourTeam,
                           Name = Name,
                           MemberPhoto = ImageHelper.ToByteArrayB(MemberPhoto)
                       };
        }*/
    }
}
