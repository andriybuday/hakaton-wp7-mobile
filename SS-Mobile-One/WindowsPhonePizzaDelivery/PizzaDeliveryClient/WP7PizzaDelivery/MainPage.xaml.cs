using System;
using System.Collections.Generic;
using System.Device.Location;
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

namespace WP7PizzaDelivery
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoPositionStatus _previousPositionStatus;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void ApplicationBarIconButton_Start(object sender, EventArgs e)
        {
            CoordinateLocator.Instance.Start(1);

            CoordinateLocator.Instance.CoordinatesUpdated += Instance_CoordinatesUpdated;
            CoordinateLocator.Instance.StatusUpdated += Instance_StatusUpdated;

        }

        private void ApplicationBarIconButton_Stop(object sender, EventArgs e)
        {
            CoordinateLocator.Instance.Stop();
            CoordinateLocator.Instance.CoordinatesUpdated -= Instance_CoordinatesUpdated;
            CoordinateLocator.Instance.StatusUpdated -= Instance_StatusUpdated;
        }

        void Instance_StatusUpdated(object sender, System.Device.Location.GeoPositionStatusChangedEventArgs e)
        {
            UpdateLabels();
            //tbDeviceStatus.Text = e.Status.ToString();
        }

        void Instance_CoordinatesUpdated(object sender, System.Device.Location.GeoPositionChangedEventArgs<System.Device.Location.GeoCoordinate> e)
        {
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            tbDeviceStatus.Text = string.Format("{0}->{1}", _previousPositionStatus, CoordinateLocator.Instance.LastStatus);
            _previousPositionStatus = CoordinateLocator.Instance.LastStatus;
            if (CoordinateLocator.Instance.LastPosition != null)
            {
                tbTimeAcquired.Text = CoordinateLocator.Instance.LastPosition.Timestamp.ToString();
                tbLatitude.Text = CoordinateLocator.Instance.LastPosition.Location.Latitude.ToString();
                tbLongtitude.Text = CoordinateLocator.Instance.LastPosition.Location.Longitude.ToString();
            }
            if (CoordinateLocator.Instance.LastLocation != null)
            {
                tbDeviceLastLocation.Text = CoordinateLocator.Instance.LastLocation.ToString();
            }
            else
            {
                tbDeviceLastLocation.Text = "NULL!";
            }
        }
    }
}