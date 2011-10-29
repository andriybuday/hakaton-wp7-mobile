using Microsoft.Phone.Controls;

namespace sdkSilverlightXNACS
{
    public partial class SearchingForOpponent : PhoneApplicationPage
    {       
        // Constructor
        public SearchingForOpponent()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
         

        }

        private void WaitingForOpponent()
        {
            //TODO: Wait for opponent
            {
                
            }

            var success = false;
            PushNotification(success ? "Opponent connected" : "No opponents on the line");
        }

        private void PushNotification(string message)
        {
            //TODO: Push notification to UI
        }
    }
}
