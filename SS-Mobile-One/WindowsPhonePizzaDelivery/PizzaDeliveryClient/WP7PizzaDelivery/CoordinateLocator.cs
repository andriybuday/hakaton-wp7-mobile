/////////////////////////////////////////////////////////////////////////////////////////
// define GPS EMULATOR when working with Windows Phone GPS Emulator to simulate location 
//#define GPS_EMULATOR
////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Device.Location;
using System.Windows.Threading;

namespace WP7PizzaDelivery
{
    public interface ICoordinateLocator : IDisposable
    {
        bool IsEnabledForUse { get; }
        int IntervalInMinutes { get; }
        GeoCoordinate LastLocation { get; }
        GeoPositionStatus LastStatus { get; }
        event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> CoordinatesUpdated;
        event EventHandler<GeoPositionStatusChangedEventArgs> StatusUpdated;

        void Start(Int32 intervalInMinutes);
        void Stop();
    }

    public class CoordinateLocator : ICoordinateLocator
    {
        private bool _isStarted = false;
        private const int GpsExpirationPeriodInMinutes = 4;

        private CoordinateLocator()
        {
#if GPS_EMULATOR
            Watcher = new GpsEmulatorClient.GeoCoordinateWatcher();
#else
            Watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
#endif
            //Watcher = new GeoCoordinateWatcher( GeoPositionAccuracy.High );
            //Watcher.PositionChanged += PositionChanged;
            Watcher.StatusChanged += StatusChanged;
        }

        private static CoordinateLocator _coordinateLocatorInstance;
        public static CoordinateLocator Instance
        {
            get { return _coordinateLocatorInstance ?? (_coordinateLocatorInstance = new CoordinateLocator()); }
        }


        public IGeoPositionWatcher<GeoCoordinate> Watcher { get; private set; }
        public DispatcherTimer DispatcherTimer { get; private set; }

        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> CoordinatesUpdated;
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusUpdated;

        public void Start(Int32 intervalInMinutes)
        {
            if (!_isStarted)
            {
                IntervalInMinutes = intervalInMinutes;
                IsEnabledForUse = true;

                DispatcherTimer = new DispatcherTimer
                                      {
                                          Interval = new TimeSpan(0, 0, 0)
                                          // default to NOW and we will set to the real value after the tick
                                      };

                DispatcherTimer.Tick += TimerTick;

                DispatcherTimer.Start();
                _isStarted = true;
            }
        }

        public void Stop()
        {
            if (DispatcherTimer != null)
            {
                DispatcherTimer.Tick -= TimerTick;
                DispatcherTimer.Stop();
            }

            if (Watcher != null && Watcher.Status != GeoPositionStatus.Disabled)
            {
                Watcher.Stop();
            }
            _isStarted = false;
        }

        public bool IsEnabledForUse { get; private set; }
        public int IntervalInMinutes { get; private set; }
        public GeoPosition<GeoCoordinate> LastPosition { get; private set; }

        public DateTimeOffset _lastTimePositionWasAvaliable = new DateTimeOffset(new DateTime(1900,1,1));

        public GeoCoordinate LastLocation
        {
            get
            {
                if (LastPosition != null)
                {
                    var timeDiffToPrevGpsSnapshot = DateTimeOffset.Now - _lastTimePositionWasAvaliable;
                    if (timeDiffToPrevGpsSnapshot.Minutes < GpsExpirationPeriodInMinutes)
                    {
                        return LastPosition.Location;
                    }
                }
                return null;
            }
        }

        public GeoPositionStatus LastStatus { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void RaiseCoordinatesUpdated(GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            if (CoordinatesUpdated != null)
            {
                CoordinatesUpdated(this, args);
            }
        }

        private void RaiseStatusUpdated(GeoPositionStatusChangedEventArgs args)
        {
            if (StatusUpdated != null)
            {
                StatusUpdated(this, args);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Watcher.Start();

            DispatcherTimer.Interval = new TimeSpan(0, IntervalInMinutes, 0);
        }

        private void StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            {
                LastPosition = Watcher.Position;
                _lastTimePositionWasAvaliable = DateTimeOffset.Now;
                //Stop the Location Service to conserve battery power.
                Watcher.Stop();
            }
            LastStatus = e.Status;
            RaiseStatusUpdated(e);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Watcher != null)
                {
                    Watcher.Stop();
                }

                if (DispatcherTimer != null)
                {
                    DispatcherTimer.Stop();
                }
            }
        }
    }
}