using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
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
            //Toilets = new ObservableCollection<Toilet> { new Toilet() { Name = "First toilet", Coordinate = new GeoCoordinate(49.845732, 24.030533) } };
            //Toilets.Add(new Toilet() { Name = "Second one", Coordinate = new GeoCoordinate(49.828952, 23.990171) });
            //Toilets.Add(new Toilet() { Name = "Third one", Coordinate = new GeoCoordinate(49.831561, 23.996458) });
           // WCServiceClient = new WCEmergencyServiceClient();
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
                _toilets.Add(new ToiletViewItem(new Toilet(), null)
                                 {Coordinate = CurrentPosition, Text = "You", Color = "Blue"});
                NotifyPropertyChanged("Toilets");
            }
        }

        private ToiletViewItem _toilet;
        public ToiletViewItem Toilet
        {
            get { return _toilet; }
            set
            {
                _toilet = value;
                _toilet.Color = "Red";
                Toilets = _toilet.Items;
                NotifyPropertyChanged("Toilet");
            }
        }

    }
}
