using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MiniGame.DataModel;
using sdkSilverlightXNACS.Storage;

namespace sdkSilverlightXNACS
{
    public partial class Team : PhoneApplicationPage   
    {
        //Variables for the application bar buttons
        ApplicationBarIconButton btnAddTeamMember;
        ApplicationBarIconButton btnAddBackground;
        ApplicationBarIconButton btnStart;
        // Constructor
        public Team()
        {
            DataContext = this;
            InitializeComponent();
            
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;


            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            //This code creates the application bar icon buttons.
            btnAddTeamMember = new ApplicationBarIconButton(new Uri("/Icons/AppBarAdd.png", UriKind.Relative));
            btnAddBackground = new ApplicationBarIconButton(new Uri("/Icons/AppBarAdd.png", UriKind.Relative));
            btnStart = new ApplicationBarIconButton(new Uri("/Icons/AppBarStart.png", UriKind.Relative));
            

            //Labels for the application bar buttons.
            btnAddTeamMember.Text = "Add Hero";
            btnAddBackground.Text = "Add location";
            btnStart.Text = "Start";

            //This code adds buttons to application bar.
            ApplicationBar.Buttons.Add(btnAddTeamMember);
            ApplicationBar.Buttons.Add(btnAddBackground);
            ApplicationBar.Buttons.Add(btnStart);

            btnAddBackground.IsEnabled = false;

            //This code will create event handlers for buttons.
            btnAddTeamMember.Click += AddNewTeamMember;
            btnAddBackground.Click += AddBackground;
            btnStart.Click += StartGame;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            TeamMembers = GameState.GetInstance().FriendsTeam;
            EnemyMembers = GameState.GetInstance().EnemyTeam;
 	        base.OnNavigatedTo(e);
        }

        public IList<Hero> TeamMembers { get; set; }
        public IList<Hero> EnemyMembers { get; set; }

        private void AddNewTeamMember(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri(string.Format("/AddMemberPhoto.xaml?teamNumber={0}",TeamsPivot.SelectedIndex), UriKind.Relative));
        }

        private void AddBackground(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/AddMemberPhoto.xaml?capture=Background", UriKind.Relative));
        }

        private void StartGame(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml?capture=Background", UriKind.Relative));
        }
    }
}
