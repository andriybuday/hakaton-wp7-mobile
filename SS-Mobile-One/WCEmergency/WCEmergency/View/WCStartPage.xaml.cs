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
using Microsoft.Phone.Controls;
using WCEmergency.Extentions;
using WCEmergency.UserControls;

namespace WCEmergency.View
{
    public partial class WcStartPage : PhoneApplicationPage 
    {
        public WcStartPage()
        {
            InitializeComponent();
            CreateAppBar();
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
                .WithButton("Accept", new Uri("/Images/AppBarCheck.png", UriKind.Relative), true, OnAccept, "Accept");


            var completedBar = builder.CompletedAppBar();
            completedBar.IsVisible = true;
            ApplicationBar = completedBar;
        }

        private void OnAccept(object arg1, System.EventArgs arg2)
        {
           
        }
    }
}