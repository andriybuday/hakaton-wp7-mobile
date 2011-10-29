using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.Core;

namespace WCEmergency.ViewModel
{
    public class MapViewModel
    {
        private const string Id = "Arjbk1wnnBh2s_MwCqJiRJtYjUuaR7fYSK2i-epPkPgijSI1FiL7XWt6WHbTk1NO";
        private readonly CredentialsProvider credentialsProvider = new ApplicationIdCredentialsProvider(Id);

        public CredentialsProvider CredentialsProvider
        {
            get { return credentialsProvider; }
        }
    }
}
