using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Allscripts.Homecare.Mobile.Device.WinPhone7UI.Controls;
using Microsoft.Phone.Controls;
using WCEmergency.Common;
using WCEmergency.Extentions;
using WCEmergency.UserControls;
using WCEmergency.ViewModel;

namespace WCEmergency.View 
{
    public partial class WcStartPage : PhoneApplicationPage
    {
        public WcStartPage()
        {
            InitializeComponent();
            CreateAppBar();

            hourSelector = new TimeSpanLoopingSelector();
            minuteSelector = new TimeSpanLoopingSelector();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //InitializeComponent();
            /*hourSelector = null;
            minuteSelector = null;
            */
            /*
            var viewModel = LayoutRoot.DataContext as WcStartPageViewModel;
            if (viewModel != null)
            {
                viewModel.Hours = CurrentUserContext.Instance.CanWait.Hours;
                viewModel.Minutes = CurrentUserContext.Instance.CanWait.Minutes;
            }

            // hourSelector.IsExpanded = false;
            // minuteSelector.IsExpanded = false;*/
        }

        private void OnMapClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(Constants.MapView, UriKind.Relative));
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

        private void OnAccept(object arg1, System.EventArgs arg2)
        {
            var viewModel = LayoutRoot.DataContext as WcStartPageViewModel;
            if (viewModel != null)
            {
                CurrentUserContext.Instance.CanWait = new TimeSpan(viewModel.Hours, viewModel.Minutes, 0);
            }

            NavigationService.Navigate(new Uri(Constants.SpeedView, UriKind.Relative));
        }
    }
}