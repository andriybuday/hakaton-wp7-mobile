using System;
using System.Windows;
using Microsoft.Phone.Controls;
using WCEmergency.View;

namespace WCEmergency
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnMapClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(Constants.MapView, UriKind.Relative));
        }
    }
}