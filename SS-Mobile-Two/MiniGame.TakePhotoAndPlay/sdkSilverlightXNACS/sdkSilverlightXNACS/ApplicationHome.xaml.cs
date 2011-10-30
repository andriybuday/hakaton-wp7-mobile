using System;
using Microsoft.Phone.Controls;

namespace sdkPhotosCS
{
    public partial class ApplicationHome : PhoneApplicationPage
    {      
        public ApplicationHome()
        {
            InitializeComponent();
        }

        private void NavigateToSinglePlayerMode(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Team.xaml?mode=SinglePlayer", UriKind.Relative));
        }

        private void NavigateToMultiPlayerMode(object sender, System.Windows.RoutedEventArgs e)
        {
            //TODO: Call service to register there
            NavigationService.Navigate(new Uri("/Team.xaml?mode=MultiPlayer", UriKind.Relative));
        }
    }
}
