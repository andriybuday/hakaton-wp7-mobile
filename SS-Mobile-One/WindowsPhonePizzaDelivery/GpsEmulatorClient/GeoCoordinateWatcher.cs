using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Device.Location;
using System.Security;
using System.Threading;
using GpsEmulatorClient.ServiceReference1;
using System.Net.Browser;

namespace GpsEmulatorClient
{
    public class GeoCoordinateWatcher : IGeoPositionWatcher<GeoCoordinate>
    {
        GpsEmulatorServiceClient client;
        GeoPosition<GeoCoordinate> currentLocation;
        
        /// <summary>
        /// Instantiates a new instance of the GeoCoordinateWatcher
        /// class with the DesiredAccuracy value of System.Device.Location.GeoPositionAccuracy.Default.
        /// </summary>
        public GeoCoordinateWatcher() : this(GeoPositionAccuracy.Default)
        {
        }

        /// <summary>
        ///  Instantiates a new instance of the GeoCoordinateWatcher class with the provided
        ///  System.Device.Location.GeoCoordinateWatcher.DesiredAccuracy value.
        /// </summary>
        /// <param name="desiredAccuracy">
        /// A member of the System.Device.Location.GeoPositionAccuracy enumeration specifying
        /// the DesiredAccuracy value for the GpsEmulatorClient.GeoCoordinateWatcher.
        /// </param>
        public GeoCoordinateWatcher(GeoPositionAccuracy desiredAccuracy)
        {
            this.desiredAccuracy = desiredAccuracy;
        }

        GeoPositionAccuracy desiredAccuracy;
        /// <summary>
        ///  The desired accuracy for data returned from the location service.
        /// </summary>
        public GeoPositionAccuracy DesiredAccuracy
        {
            get { return desiredAccuracy; }
        }

        double momementThreshold = 1;
        /// <summary>
        /// The minimum distance that must be travelled between successive PositionChanged events.
        /// </summary>
        public double MovementThreshold
        {
            get { return momementThreshold; }
            set {
                if (momementThreshold < 0) throw new ArgumentException("MovementThreshold value must be greater than zero");
                momementThreshold = value;
            }
        }

        /// <summary>
        /// The application’s level of access to the location service.
        /// In the case of the Emulator, this always returns GeoPositionPermission.Granted.
        /// </summary>
        /// <remarks>Note this is specific Emulator behaivor</remarks>
        public GeoPositionPermission Permission
        {
            get { return GeoPositionPermission.Granted; }
        }

        /// <summary>
        /// The most recent position obtained from the location service.
        /// </summary>
        public GeoPosition<GeoCoordinate> Position
        {
            get
            {
                return currentLocation;
            }
        }


        GeoPositionStatus status = GeoPositionStatus.Disabled;
        /// <summary>
        /// The status of the location service, determined by the abuility to connect to the GPS Emulator.
        /// </summary>
        /// <remarks>In real WP this is determined by the ability to find location</remarks>
        public GeoPositionStatus Status
        {
            get { return status; }
            private set
            {
                if (status != value)
                {
                    this.status = value;
                    OnStatusChanged(new GeoPositionStatusChangedEventArgs(status));
                }
            }
        }

        /// <summary>
        /// Occurs when the location service detects a change in position, in accordance with the limits specified by the MovementThreshold value.
        /// </summary>
        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;

        /// <summary>
        /// Occurs when the status of the location service changes.
        /// </summary>
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Releases resources used by the GeoCoordinateWatcher and stops the acquisition of data from the GPS Emulator.
        /// </summary>
        [SecuritySafeCritical]
        public void Dispose()
        {
            if (client != null)
            {
                try
                {
                    if (client.State != CommunicationState.Faulted) client.CloseAsync(TimeSpan.FromSeconds(5));
                    Status = GeoPositionStatus.Disabled;
                }
                finally
                {
                    client = null;
                }
            }
        }

        /// <summary>
        /// Raises the PositionChanged event.
        /// </summary>
        /// <param name="e">The event data</param>
        protected void OnPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (PositionChanged != null) PositionChanged(this, e);
        }

        /// <summary>
        /// Raises the StatusChanged event.
        /// </summary>
        /// <param name="e">The event data</param>
        protected void OnStatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            if (StatusChanged != null) StatusChanged(this, e);
        }

        /// <summary>
        /// Starts the acquisition of data from the GPS Emulator.
        /// </summary>
        /// <remarks></remarks>
        [SecuritySafeCritical]
        public void Start()
        {
            if (client == null)
            {
                try
                {
                    status = GeoPositionStatus.Initializing;
                    client = new GpsEmulatorServiceClient( 
                                    new BasicHttpBinding(BasicHttpSecurityMode.None), 
                                    new EndpointAddress("http://localhost:8192/GpsEmulator")); // change end point to real IP when testing on a real device
                    client.OpenCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_OpenCompleted);
                    client.GetCurrentPositionCompleted +=new EventHandler<GetCurrentPositionCompletedEventArgs>(client_GetCurrentPositionCompleted);
                    ICommunicationObject commObject = client as ICommunicationObject;
                    if (commObject != null)
                    {
                        commObject.Faulted += new EventHandler((a,b) => { Stop(); });
                    }
                    client.GetCurrentPositionAsync();
                }
                catch
                {
                    client = null;
                    Status = GeoPositionStatus.Disabled;
                }
            }
        }

        void client_OpenCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Status = GeoPositionStatus.Ready;
        }
   
        /// <summary>
        /// Starts the acquisition of data from the GPS Emulator.
        /// </summary>
        /// <param name="suppressPermissionPrompt">This parameter is not used.</param>
        [SecuritySafeCritical]
        public void Start(bool suppressPermissionPrompt)
        {
            Start();
        }

        /// <summary>
        /// Stops the acquisition of data from the GPS Emulator.
        /// </summary>
        [SecuritySafeCritical]
        public void Stop()
        {
            Dispose();
        }
        //
        // Summary:
        //     Attempts to start the acquisition of data from the location service. If the
        //     provided timeout interval is exceeded before the location service responds,
        //     the request for location is stopped and the method returns false.
        //
        // Parameters:
        //   suppressPermissionPrompt:
        //     This parameter is not used.
        //
        //   timeout:
        //     A TimeSpan object specifying the amount of time to wait for location data
        //     acquisition to begin.
        //
        // Returns:
        //     Returns System.Boolean . true if the location service responds within the
        //     timeout window. Otherwise, false.
        [SecuritySafeCritical]
        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            Start();
            return true;
        }

        static char[] latLngSeparator = new char[] { ';' };
        static GeoCoordinate _PrevGc = null;

        void client_GetCurrentPositionCompleted(object sender, GetCurrentPositionCompletedEventArgs e)
        {
            
            if (client == null) return;
            if (e.Error != null)
            {
                Status = GeoPositionStatus.Disabled;
                client = null;
                return;
            }

            string[] coordinates = e.Result.Split(latLngSeparator);
            if (coordinates.Length == 2)
            {
                double lat, lng;
                if (Double.TryParse(coordinates[0], out lat) && double.TryParse(coordinates[1], out lng))
                {
                    GeoCoordinate gc = new GeoCoordinate(lat, lng);
                    if (_PrevGc == null || !_PrevGc.Equals(gc))
                    {
                        _PrevGc = gc;
                        currentLocation = new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, gc);
                        GeoPositionChangedEventArgs<GeoCoordinate> gpcea = new GeoPositionChangedEventArgs<GeoCoordinate>(currentLocation);
                        Status = GeoPositionStatus.Ready;
                        OnPositionChanged(gpcea);
                    }
                }
                else
                {
                }
            }
            else
            {
                Status = GeoPositionStatus.NoData;
            }

            if (client != null)
            {
                client.GetCurrentPositionAsync();    
            }
        }

    }
}
