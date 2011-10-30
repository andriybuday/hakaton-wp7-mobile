using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WCEmergency.Common;
using WCEmergency.Extentions;
using WCEmergency.UserControls;
using WCEmergency.ViewModel;

namespace WCEmergency.View
{
    public partial class SpeedView : PhoneApplicationPage
    {
        public SpeedView()
        {
            InitializeComponent();
            CreateAppBar();
        }
        
        protected void CreateAppBar()
        {
            if (ApplicationBar != null)
                return;

            ApplicationBarBuilder builder = new BindableApplicationBarBuilder();

            builder = builder.Create()
                .WithBackground(0x4D, 0x4D, 0x4D) //FF4D4D4D
                .WithButton("Accept", new Uri("/Resources/Images/AppBarCheck.png", UriKind.Relative), true, OnAccept, "Accept");


            var completedBar = builder.CompletedAppBar();
            completedBar.IsVisible = true;
            ApplicationBar = completedBar;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var viewModel = LayoutRoot.DataContext as WcStartPageViewModel;
            if (viewModel != null)
            {
                viewModel.Hours = CurrentUserContext.Instance.Speed;
            }
        }

        private void OnAccept(object arg1, System.EventArgs arg2)
        {
            var viewModel = LayoutRoot.DataContext as SpeedViewModel;
            if (viewModel != null)
            {
                CurrentUserContext.Instance.Speed = viewModel.Speed;
            }

            NavigationService.Navigate(new Uri(Constants.ToiletView, UriKind.Relative)); 
        }
    }
}