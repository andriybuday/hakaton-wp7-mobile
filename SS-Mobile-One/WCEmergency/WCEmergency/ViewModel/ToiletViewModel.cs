using System.Collections.ObjectModel;
using System.Device.Location;
using WCEmergency.WCServiceReference;

namespace WCEmergency.ViewModel
{
    public class ToiletViewModel
    {
        private WCEmergencyServiceClient WCServiceClient { get; set; }
        private GeoCoordinateWatcher Watcher { get; set; }

        public ToiletViewModel()
        {
            InitializeWatcher();
            WCServiceClient = new WCEmergencyServiceClient();
        }

        private void InitializeWatcher()
        {
            Watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default) { MovementThreshold = 1 };
            Watcher.PositionChanged += OnWatcherPositionChanged;
            Watcher.Start();
        }

        private void OnWatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            if (args != null || !args.Position.Location.IsUnknown)
            {
                var currentPosition = new GeoCoordinate(args.Position.Location.Latitude, args.Position.Location.Longitude, args.Position.Location.Altitude);

                WCServiceClient.GetNearestToiltesCompleted += OnGetNearestToiltesCompleted;
                WCServiceClient.GetNearestToiltesAsync(args.Position.Location, 0);
            }
        }

        private void OnGetNearestToiltesCompleted(object sender, GetNearestToiltesCompletedEventArgs e)
        {
            Toilets = e.Result;
            WCServiceClient.GetNearestToiltesCompleted -= OnGetNearestToiltesCompleted;
            Watcher.Stop();
        }

        public ObservableCollection<Toilet> Toilets { get; set; }
    }
}
