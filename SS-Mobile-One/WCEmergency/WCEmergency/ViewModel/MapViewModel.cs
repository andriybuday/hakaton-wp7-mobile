using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Windows.Media;
using Microsoft.Phone.Controls.Maps;
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

    }
}
