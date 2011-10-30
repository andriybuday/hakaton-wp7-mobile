using System;
using Microsoft.Phone.Controls;
using sdkSilverlightXNACS.Storage;

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
            var service = new sdkSilverlightXNACS.MiniGameService.MiniGameServiceClient();

            service.RegisterMeCompleted += ServiceRegisterMeCompleted;
            service.RegisterMeAsync();
            NavigationService.Navigate(new Uri("/Team.xaml?mode=MultiPlayer", UriKind.Relative));
        }

        void ServiceRegisterMeCompleted(object sender, sdkSilverlightXNACS.MiniGameService.RegisterMeCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GameState.GetInstance().TeamName = e.Result;
            }
        }
    }
}
