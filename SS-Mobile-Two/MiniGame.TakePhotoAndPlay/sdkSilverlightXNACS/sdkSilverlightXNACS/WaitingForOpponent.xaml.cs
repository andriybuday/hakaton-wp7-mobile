using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using MiniGame.DataModel;
using sdkSilverlightXNACS.Storage;

namespace sdkSilverlightXNACS
{
    public partial class WaitingForOpponent : PhoneApplicationPage
    {
        private Timer _player2Timer;
        // Constructor
        public WaitingForOpponent()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
         
            _player2Timer = new Timer(WaitForOpponent,null, 10000, 2000);
        }

        private void WaitForOpponent(object sender)
        {
            var service = new MiniGameService.MiniGameServiceClient();

            service.GetEnemyTeamCompleted += service_GetEnemyTeamCompleted;
            service.GetEnemyTeamAsync(GameState.GetInstance().TeamName);
        }

        void service_GetEnemyTeamCompleted(object sender, MiniGameService.GetEnemyTeamCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GameState.GetInstance().EnemyTeam = e.Result.Heros.Select(x =>new Hero()
                            {
                                IsInYourTeam = false,
                                Name = x.Name,
                                MemberPhoto = x.MemberPhoto
                            }).ToList();

                if (GameState.GetInstance().EnemyTeam.Count > 0)
                {

                    GameState.GetInstance().IsGameStarted = true;
                    GameState.GetInstance().TimeGameStarted = DateTime.Now;
                    Player2IsReady();
                }
            }
        }

        private void Player2IsReady()
        {
            this.Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative)));
        }
    }
}
