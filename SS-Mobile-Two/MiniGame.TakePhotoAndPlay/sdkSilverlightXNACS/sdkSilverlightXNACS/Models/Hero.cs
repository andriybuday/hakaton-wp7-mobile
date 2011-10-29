using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace sdkSilverlightXNACS.Models
{
    
    public class Hero
    {
        public bool IsInYourTeam { get; set; }
        public WriteableBitmap MemberPhoto { get; set; }
        public string Name { get; set; }
    }
}
