using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using sdkSilverlightXNACS.Models;
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
            btnAddTeamMember = new ApplicationBarIconButton(new Uri("/Icons/appbar.edit.rest.png", UriKind.Relative));
            btnAddBackground = new ApplicationBarIconButton(new Uri("/Icons/appbar.check.rest.png", UriKind.Relative));
            btnStart = new ApplicationBarIconButton(new Uri("/Icons/appbar.save.rest.png", UriKind.Relative));
            

            //Labels for the application bar buttons.
            btnAddTeamMember.Text = "Add Hero";
            btnAddBackground.Text = "Add location";
            btnStart.Text = "Start";

            //This code adds buttons to application bar.
            ApplicationBar.Buttons.Add(btnAddTeamMember);
            ApplicationBar.Buttons.Add(btnAddBackground);
            ApplicationBar.Buttons.Add(btnStart);

            //This code will create event handlers for buttons.
            btnAddTeamMember.Click += AddNewTeamMember;
            btnAddBackground.Click += AddBackground;
            btnStart.Click += StartGame;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            TeamMembers = GameState.GetInstance().MyTeam;
 	        base.OnNavigatedTo(e);
        }

        public IList<Hero> TeamMembers { get; set; }

        private void AddNewTeamMember(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void AddBackground(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml?capture=Background", UriKind.Relative));
        }

        private void StartGame(object sender, EventArgs args)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml?capture=Background", UriKind.Relative));
        }
    }
}
