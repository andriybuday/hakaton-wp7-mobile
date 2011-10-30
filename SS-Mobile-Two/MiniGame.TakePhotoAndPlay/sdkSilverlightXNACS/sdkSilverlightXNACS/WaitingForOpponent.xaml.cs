using System;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;

namespace sdkSilverlightXNACS
{
    public partial class WaitingForOpponent : PhoneApplicationPage
    {       
        // Constructor
        public WaitingForOpponent()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
         
            WaitForOpponent();
        }

        private void WaitForOpponent()
        {
            //TODO: call service to know if opponent is on line
            Player2IsReady();
        }

        private void Player2IsReady()
        {
            MessageBox.Show("Other player is ready. Starting game");
            //NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}
