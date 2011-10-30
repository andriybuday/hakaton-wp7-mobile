using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using WCEmergency.Bing.Route;
using WCEmergency.Helpers;
using WCEmergency.Models;
using WCEmergency.WCServiceReference;

namespace WCEmergency.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel()
        {
            InitializeWatcher();
        }

       // private WCEmergencyServiceClient WCServiceClient { get; set; }
        private GeoCoordinateWatcher Watcher { get; set; }

        private void InitializeWatcher()
        {
            Watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) { MovementThreshold = 1 };
            Watcher.PositionChanged += OnWatcherPositionChanged;
            Watcher.StatusChanged += OnWatcherStatusChanged;
            Watcher.Start();
        }

        private void OnWatcherStatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case GeoPositionStatus.Disabled:
                    //
                    break;
                case GeoPositionStatus.NoData:
                    // data unavailable
                break;
            }
        }

        private void OnWatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            if(CurrentPosition == null || CurrentPosition.IsUnknown)
            {
               
            }
            CurrentPosition = new GeoCoordinate(args.Position.Location.Latitude, args.Position.Location.Longitude, args.Position.Location.Altitude);
            CurrentView = CurrentPosition;
            RequestRoute();
            //WCServiceClient.GetNearestToiltesCompleted += OnGetNearestToiltesCompleted;
            //WCServiceClient.GetNearestToiltesAsync(args.Position.Location, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private const string Id = "Arjbk1wnnBh2s_MwCqJiRJtYjUuaR7fYSK2i-epPkPgijSI1FiL7XWt6WHbTk1NO";
        private readonly CredentialsProvider credentialsProvider = new ApplicationIdCredentialsProvider(Id);

        public CredentialsProvider CredentialsProvider
        {
            get { return credentialsProvider; }
        }

        private GeoCoordinate _currentPosition;
        public GeoCoordinate CurrentPosition
        {
            get
            {
               return _currentPosition;
            }
            set
            {
                _currentPosition = value;
            
                if (CurrentToilet == null)
                {
                   CurrentToilet = new ToiletViewItem(new Toilet(){Coordinate = CurrentPosition},null);
                }
                else
                {
                    CurrentToilet.Coordinate = CurrentPosition;
                }
                
                NotifyPropertyChanged("CurrentPosition");
            }
        }

        private ObservableCollection<ToiletViewItem> _toilets;
        public ObservableCollection<ToiletViewItem> Toilets
        {
            get { return _toilets; }
            set
            {
                _toilets = value;

                NotifyPropertyChanged("Toilets");
            }
        }

        private ToiletViewItem _targetToilet;
        public ToiletViewItem TargetToilet
        {
            get { return _targetToilet; }
            set
            {
                _targetToilet = value;
                Toilets = _targetToilet.Items;
                foreach (var toiletViewItem in Toilets)
                {
                    toiletViewItem.Color = "Green";
                }
            
                _targetToilet.Color = "Red";
            }
        }

        private ToiletViewItem _currentToilet;
        public ToiletViewItem CurrentToilet
        {
            get { return _currentToilet; }
            set
            {
                _currentToilet = value;
                _currentToilet.Color = "Blue";
                _currentToilet.Text = "You";
                Toilets.Add(_currentToilet);
                NotifyPropertyChanged("Toilets");
            }
        }

        private GeoCoordinate _currentView;
        public GeoCoordinate CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                NotifyPropertyChanged("CurrentView");
            }
        }


        //-----------------------------------------------
        private void RequestRoute()
        {
            var routeRequest = new RouteRequest();
            routeRequest.Credentials = new Credentials();
            routeRequest.Credentials.ApplicationId = Id;
            routeRequest.Waypoints = new ObservableCollection<Waypoint>();
            routeRequest.Waypoints.Add(new Waypoint(){Location = new Location(){ Altitude = CurrentPosition.Altitude, Latitude = CurrentPosition.Latitude, Longitude = CurrentPosition.Longitude}});
            routeRequest.Waypoints.Add(new Waypoint() { Location = new Location() { Altitude = TargetToilet.Coordinate.Altitude, Latitude = TargetToilet.Coordinate.Latitude, Longitude = CurrentPosition.Longitude} });
            routeRequest.Options = new RouteOptions();
            routeRequest.Options.RoutePathType = RoutePathType.Points;
            routeRequest.UserProfile = new UserProfile();
            routeRequest.UserProfile.DistanceUnit = DistanceUnit.Kilometer;

            // Execute the request. 
            var routeClient = new RouteServiceClient("BasicHttpBinding_IRouteService");
            routeClient.CalculateRouteCompleted += OnRouteComplete;
            routeClient.CalculateRouteAsync(routeRequest);
        }

        private void OnRouteComplete(object sender, CalculateRouteCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Result != null && e.Result.Result.Legs != null & e.Result.Result.Legs.Any())
            {
                var result = e.Result.Result;
                var legs = result.Legs.FirstOrDefault();

                //StartPoint = legs.ActualStart;
                //EndPoint = legs.ActualEnd;
                var locations = result.RoutePath.Points;
                var coordinates = new LocationCollection();
                foreach (var location in locations)
                {
                    coordinates.Add(new GeoCoordinate(){Altitude = location.Altitude, Latitude = location.Latitude, Longitude = location.Longitude});
                }
                RoutePoints = coordinates;
                
               // Itinerary = legs.Itinerary;
            }
        }

        private LocationCollection _routePoints;
        public LocationCollection RoutePoints
        {
            get { return _routePoints; }
            set 
            { 
                _routePoints = value;
                NotifyPropertyChanged("RoutePoints");
            }
        }

    }
}
