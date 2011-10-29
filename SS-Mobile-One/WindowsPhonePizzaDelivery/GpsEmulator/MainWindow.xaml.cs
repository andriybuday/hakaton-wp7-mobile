using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GpsEmulator.BingApis;
using GpsEmulator.MapControl;
using GpsEmulator.Markers;
using GpsEmulator.Utilities;
using GpsEmulator.GpsService;
using Microsoft.Win32;
using System.ServiceModel;
using System.Windows.Ink;

namespace GpsEmulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public partial class MainWindow : Window, IGpsEmulatorService
    {
        ObservableCollection<TimedPosition> route = new ObservableCollection<TimedPosition>();
        static string MSG_BOX_CAPTION = "Windows Phone 7 GPS Emulator";
        string bingApiKey;
        BingApis.BingMapsClient bingMapsClient;
        double defaultSpeed; // In meters per second
        bool useRealTimeData = false;
        string transmittedLocation = "Not Started";
        bool clientCallDetected = false;
        ServiceHost host;

        public MainWindow()
        {
            try
            {
                // setup the GPS host service 
                host = new ServiceHost(this, new Uri("http://localhost:8192/"));
                host.AddServiceEndpoint(typeof(IGpsEmulatorService), new BasicHttpBinding(BasicHttpSecurityMode.None), "GpsEmulator");
                host.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start the GPS Emulator!\nPlease make sure the application has Administrator privilages.\n\nError details: " + ex.Message, MSG_BOX_CAPTION, MessageBoxButton.OK);
                Application.Current.Shutdown() ;
            }

            #region Read application settings from configuration file
            double lastLat, lastLng;
            int lastZoom, lastMapType;
            int windowTop, windowLeft, windowHeight, windowWidth;
            bingApiKey = ConfigurationManager.AppSettings["BingApiKey"];
            bingMapsClient = new BingApis.BingMapsClient(bingApiKey);
            if (!Double.TryParse(ConfigurationManager.AppSettings["DefaultSpeed"], out defaultSpeed)) defaultSpeed = 16.6666;
            if (!Boolean.TryParse(ConfigurationManager.AppSettings["UseRealTimeData"], out useRealTimeData)) useRealTimeData = false;
            #endregion

            InitializeComponent();

            #region BindMenuCommends

            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenCommandHandler, null));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, NewCommendHandler, null));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveCommandHandler, null));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseCommandHandler, null));

            #endregion

            #region Restore main window settings from the last session
            if (ConfigurationManager.AppSettings["windowMaximized"] != null && ConfigurationManager.AppSettings["windowMaximized"] == "True") this.WindowState = System.Windows.WindowState.Maximized;
            if (Int32.TryParse(ConfigurationManager.AppSettings["windowTop"], out windowTop)) this.Top = windowTop;
            if (Int32.TryParse(ConfigurationManager.AppSettings["windowLeft"], out windowLeft)) this.Left = windowLeft;
            if (Int32.TryParse(ConfigurationManager.AppSettings["windowHeight"], out windowHeight)) this.Height = windowHeight > 400 ? windowHeight : 400;
            if (Int32.TryParse(ConfigurationManager.AppSettings["windowWidth"], out windowWidth)) this.Width = windowWidth > 450 ? windowWidth : 450;
            #endregion

            #region Restore map control settings from the last session
            if (!Double.TryParse(ConfigurationManager.AppSettings["LastLocationLat"], out lastLat)) lastLat = 47.6395454;
            if (!Double.TryParse(ConfigurationManager.AppSettings["LastLocationLng"], out lastLng)) lastLng = -122.130699;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["LastZoom"], out lastZoom)) lastZoom = 17;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["LastMapType"], out lastMapType)) lastMapType = 1;
            MapControl.ZoomLevel = lastZoom;
            MapControl.MapCenter = new Point(lastLat, lastLng);
            MapControl.MapType = (MapType) lastMapType;
            if (ConfigurationManager.AppSettings["PathColor"] != null)
            {
                Brush brush;
                switch (ConfigurationManager.AppSettings["PathColor"])
                {
                    case "Green":
                        brush = Brushes.ForestGreen;
                        break;
                    case "Blue":
                        brush = Brushes.RoyalBlue;
                        break;
                    default:
                        brush = Brushes.Red;
                        break;
                }
                MapControl.PathColor = brush;
            }
            #endregion

            MapControl.MapTileFactory = new InMemoryCacheMapTileFactory( new WebMapTileFactory(bingMapsClient) );

            // Attach map control event handlers
            MapControl.MouseMove += new System.Windows.Input.MouseEventHandler(MapControl_MouseMove);
            MapControl.MouseLeftButtonDown += new MouseButtonEventHandler(MapControl_MouseLeftButtonDown);
            MapControl.MouseLeftButtonUp += new MouseButtonEventHandler(MapControl_MouseLeftButtonUp);
            MapControl.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(MapControl_MouseDoubleClick);
            MapControl.MouseWheel += new MouseWheelEventHandler(MapControl_MouseWheel);

            // Bind map type drop down
            cmbMapType.ItemsSource = Enum.GetValues(typeof(Utilities.MapType));

            // route
            lvRoute.ItemsSource = route;
            lvRoute.SizeChanged += lvRoute_SizeChanged;
            lvRoute.SelectionChanged += lvRoute_SelectionChanged;
            lvRoute.KeyUp += new KeyEventHandler(lvRoute_KeyUp);

            #region Internet Connectivity Checking
            // check internet connectivity
            Task t = Task.Factory.StartNew(() =>
            {
                //bool previouslyConnectedToTheInternet = true;
                while (true)
                {
                    ((InMemoryCacheMapTileFactory)MapControl.MapTileFactory).CleanupCache();
                    tbOnlineStatus.Dispatcher.Invoke((Action)(() => {
                        tbOnlineStatus.Text = InternetConnectivityChecker.IsConnectedToInternet() ? "Connected to the Internet" : "Not Connected to the Internet";
                        if (clientCallDetected)
                        {
                            elClientIndicator.Fill = Brushes.Green;
                            tbClientIndicator.Text = "Client connected";
                            clientCallDetected = false;
                        }
                        else
                        {
                            elClientIndicator.Fill = Brushes.Red;
                            tbClientIndicator.Text = "No client connected";
                        }
                    }));
                    // Refresh the display if connected
                    //bool connectedToTheInternet = InternetConnectivityChecker.IsConnectedToInternet();
                    //if (!previouslyConnectedToTheInternet && connectedToTheInternet)
                    //{
                    //    MapControl.InvalidateVisual();
                    //}
                    //previouslyConnectedToTheInternet = connectedToTheInternet;
                    System.Threading.Thread.Sleep(2000);
                }
            }, TaskCreationOptions.LongRunning);
            #endregion
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                host.Close(TimeSpan.FromSeconds(1));
            }
            catch {}

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            AddOrCreateConfigSection(config, "LastLocationLat", MapControl.MapCenter.X.ToString());
            AddOrCreateConfigSection(config, "LastLocationLng", MapControl.MapCenter.Y.ToString());
            AddOrCreateConfigSection(config, "LastZoom", MapControl.ZoomLevel.ToString());
            AddOrCreateConfigSection(config, "windowTop", this.Top.ToString());
            AddOrCreateConfigSection(config, "windowLeft", this.Left.ToString());
            AddOrCreateConfigSection(config, "windowHeight", this.ActualHeight.ToString());
            AddOrCreateConfigSection(config, "windowWidth", this.ActualWidth.ToString());
            AddOrCreateConfigSection(config, "windowMaximized", this.WindowState==System.Windows.WindowState.Maximized ? "True" : "False");
            AddOrCreateConfigSection(config, "LastMapType", ((int)MapControl.MapType).ToString());
            
            config.Save(ConfigurationSaveMode.Modified);            base.OnClosing(e);
        }

        private void AddOrCreateConfigSection(Configuration config, string key, string value)
        {
            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
        }

        #region Map Event Handlers

        void MapControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(MapControl);
            endPoint = MapControl.LatLongFromLocal((int)endPoint.X, (int)endPoint.Y);
            tbPositionLat.Text = endPoint.X.ToString();
            tbPositionLng.Text = endPoint.Y.ToString();
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // Add a path
                ExtendRouteFromLastPosition();
            }
            else
            {
                // Add a single point
                if (lvRoute.Items.Count > 0)
                {
                    // Update the time based on the location of the new point
                    TimedPosition tmp = lvRoute.Items[lvRoute.Items.Count - 1] as TimedPosition;
                    Point startPoint = new Point(tmp.Position.X, tmp.Position.Y);
                    double distance = MapUtils.GetDistance(startPoint, endPoint);
                    tbTime.Text = tmp.Time.Add(TimeSpan.FromMilliseconds(100 * distance / defaultSpeed)).ToString();
                }
                AddWaypointAtCurrentPosition();
            }
        }

        Point mapMouseDownPoint = new Point();
        void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mapMouseDownPoint = e.GetPosition(MapControl);
            Mouse.Capture(MapControl);
        }

        bool dragDetected = false;
        void MapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            Point p = e.GetPosition(MapControl);
            if (!dragDetected)
            {
                Point clickLatLong = MapControl.LatLongFromLocal(p.X, p.Y);
                MapControl.SetSelectedPoint(clickLatLong.X, clickLatLong.Y);
                tbPositionLat.Text = clickLatLong.X.ToString();
                tbPositionLng.Text = clickLatLong.Y.ToString();
                transmittedLocation = GetTransmittedLocation(tbPositionLat.Text, tbPositionLng.Text);
            }
            dragDetected = false;
        }

        void MapControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mapMouseDownPoint == e.GetPosition(MapControl)) return;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                dragDetected = true;
                Mouse.SetCursor(Cursors.SizeAll);
                Point p = e.GetPosition(MapControl);
                double dX = (mapMouseDownPoint.X - p.X) / 2;
                double dY = (mapMouseDownPoint.Y - p.Y) / 2;
                MapControl.MoveCenter(dX, dY);
                mapMouseDownPoint = p;
            }
        }

        void MapControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int oldZoom = MapControl.ZoomLevel;
            Point uiPoint = Mouse.GetPosition(MapControl);
            Point uiLatLng = MapControl.LatLongFromLocal(uiPoint.X, uiPoint.Y);
            Point latLngDelta = new Point(uiLatLng.X - MapControl.MapCenter.X, uiLatLng.Y - MapControl.MapCenter.Y);
            Point newCenter;
            if (e.Delta < 0) 
            {
                MapControl.ZoomLevel -= 1;
                newCenter = new Point(MapControl.MapCenter.X - latLngDelta.X, MapControl.MapCenter.Y - latLngDelta.Y);
            }
            else
            {
                MapControl.ZoomLevel += 1;
                newCenter = new Point(MapControl.MapCenter.X + latLngDelta.X/2, MapControl.MapCenter.Y + latLngDelta.Y/2);
            }
            if (oldZoom != MapControl.ZoomLevel)
            {
                MapControl.MapCenter = newCenter;
            }
        }

        #endregion

        #region Event handlers for buttons, lists, etc.
        private void btnAddPoint_Click(object sender, RoutedEventArgs e)
        {
            AddWaypointAtCurrentPosition();
        }

        private void btnAddRoute_Click(object sender, RoutedEventArgs e)
        {
            ExtendRouteFromLastPosition();
        }

        private void btnUpdatePoint_Click(object sender, RoutedEventArgs e)
        {
            int index = lvRoute.SelectedIndex;
            if (index == -1) return;

            double lat, lng;
            TimeSpan time;
            if (!Double.TryParse(tbPositionLat.Text, out lat) || !Double.TryParse(tbPositionLng.Text, out lng)) return;
            if (!TimeSpan.TryParse(tbTime.Text, out time)) return;

            TimedPosition waypoint = new TimedPosition(time, lat, lng);
            WaypointMarker newMarker = new WaypointMarker(this, String.Format("Lat  : {0}\nLong : {1}", waypoint.Position.X, waypoint.Position.Y));
            MapMarker mm = MapControl.ReplaceMarker(route[index].MapMarker, newMarker, waypoint.Position.X, waypoint.Position.Y);
            waypoint.MapMarker = mm;
            route[index] = waypoint;

            lvRoute.SelectedIndex = index;
            MapControl.RegenerateRouteShape();
            return;

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            UpdateLocationUsingSearchString();
        }

        int simulationSpeed = 1;
        Task currentNavigationTask = null;
        static CancellationTokenSource cancellationTokenSource;
        static CancellationToken canellationToken;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (currentNavigationTask != null)
            {
                cancellationTokenSource.Cancel();
                currentNavigationTask = null;
                btnStart.Content = "Start";
                btnReset.IsEnabled = true;
                return;
            }

            const int DELAY = 32;
            if (lvRoute.Items.Count < 2)
            {
                MessageBox.Show("Please add 2 or more waypoints to the route", MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            if (lvRoute.SelectedIndex == -1)
            {
                lvRoute.SelectedIndex = 0;
            }
            int index = lvRoute.SelectedIndex;
            TimeSpan currentTime = (lvRoute.Items[index] as TimedPosition).Time.Add(new TimeSpan());

            Point p = (lvRoute.Items[index] as TimedPosition).Position;
            DirectionalMarker dm = new DirectionalMarker();
            MapMarker directionalMarker = MapControl.AddMarker(dm, p.X, p.Y);
            directionalMarker.SetVisualLocation(MapControl.ZoomLevel);

            cancellationTokenSource = new CancellationTokenSource();
            canellationToken = cancellationTokenSource.Token;
            btnStart.Content = "Stop";
            btnReset.IsEnabled = false;

            currentNavigationTask = Task.Factory.StartNew(() =>
            {
                while (index < lvRoute.Items.Count - 1)
                {
                    if (canellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    TimedPosition w1 = lvRoute.Items[index] as TimedPosition;
                    TimedPosition w2 = lvRoute.Items[index + 1] as TimedPosition;
                    double numOfStepsOnLeg = (w2.Time - w1.Time).TotalMilliseconds / DELAY;
                    double dLat = (w2.Position.X - w1.Position.X) / numOfStepsOnLeg;
                    double dLng = (w2.Position.Y - w1.Position.Y) / numOfStepsOnLeg;
                    Point currentPosition = new Point(w1.Position.X, w1.Position.Y);
                    Point directionalMarkerCurrentLocation = new Point(w1.Position.X, w1.Position.Y);
                    this.Dispatcher.Invoke((Action)(() => { dm.SetHeading(dLat, dLng); }));

                    while (currentTime < w2.Time)
                    {
                        if (canellationToken.IsCancellationRequested) break;
                        Thread.Sleep(DELAY);
                        currentTime = currentTime.Add(new TimeSpan(0, 0, 0, 0, (int) ((double)DELAY * simulationSpeed)));
                        currentPosition.X += dLat * simulationSpeed;
                        currentPosition.Y += dLng * simulationSpeed;
                        directionalMarkerCurrentLocation.X += dLat * simulationSpeed;
                        directionalMarkerCurrentLocation.Y += dLng * simulationSpeed;
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            transmittedLocation = GetTransmittedLocation(currentPosition.X.ToString(), currentPosition.Y.ToString());
                            tbCurrentPosition.Text = String.Format("{0} : {1}", currentTime, transmittedLocation);
                            directionalMarker.Location = directionalMarkerCurrentLocation;
                            directionalMarker.SetVisualLocation(MapControl.ZoomLevel);
                            MapControl.MapCenter = directionalMarkerCurrentLocation;
                        }));
                    }
                    index++;
                    this.Dispatcher.Invoke((Action)(() => lvRoute.SelectedIndex = index));
                }
            })
                .ContinueWith(_ => this.Dispatcher.Invoke((Action)(() =>
                    {
                        MapControl.RemoveMarker(directionalMarker);
                        currentNavigationTask = null;
                        btnStart.Content = "Start";
                        btnReset.IsEnabled = true;
                    })));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lvRoute.SelectedIndex = 0;
            tbCurrentPosition.Text = "";
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            MapControl.ZoomLevel += 1;
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            MapControl.ZoomLevel -= 1;
        }

        private void cmbMapType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MapControl.MapType = (GpsEmulator.Utilities.MapType)cmbMapType.SelectedValue;
        }

        private void sldSimulationSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            simulationSpeed = (int)Math.Pow(2, (int)e.NewValue);
            tbSpeed.Text = String.Format("Speed : x{0}", simulationSpeed);
        }

        private void tbSearchString_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) UpdateLocationUsingSearchString();
        }

        #endregion

        #region Text box validators
        /// <summary>
        /// Ensured that a text box contains a valid Double number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            Debug.Assert(tb!=null);
            double notUsed;
            if (Double.TryParse(tb.Text, out notUsed)) tb.Background = Brushes.White;
            else tb.Background = Brushes.LightPink;
        }

        private void timeTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            Debug.Assert(tb != null);
            TimeSpan notUsed;
            if (TimeSpan.TryParse(tb.Text, out notUsed)) tb.Background = Brushes.White;
            else tb.Background = Brushes.LightPink;
        }
        #endregion

        #region Route list event handlers
        private void lvRoute_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView view = lvRoute.View as GridView;
            view.Columns[0].Width = lvRoute.ActualWidth / 3;
            view.Columns[1].Width = lvRoute.ActualWidth / 3;
            view.Columns[2].Width = lvRoute.ActualWidth / 3;
        }

        private void lvRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimedPosition tp = lvRoute.SelectedItem as TimedPosition;
            if (tp == null) return;
            tbPositionLat.Text = tp.Position.X.ToString();
            tbPositionLng.Text = tp.Position.Y.ToString();
            tbTime.Text = tp.Time.ToString();
            MapControl.MapCenter = new Point(tp.Position.X, tp.Position.Y);
            MapControl.SetSelectedPoint(tp.Position.X, tp.Position.Y);
            transmittedLocation = GetTransmittedLocation(tbPositionLat.Text, tbPositionLng.Text);
        }

        void lvRoute_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && lvRoute.SelectedItems.Count>0)
            {
                TimedPosition tp = lvRoute.SelectedItem as TimedPosition;
                while( tp != null)
                {
                    MapControl.RemoveMarker(tp.MapMarker);
                    MapControl.RegenerateRouteShape();
                    MapControl.InvalidateVisual();
                    route.Remove(tp);
                    tp = lvRoute.SelectedItem as TimedPosition;
                }
            }
        }
        #endregion

        #region Menu commands

        private void CloseCommandHandler(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenCommandHandler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Route Files|*.route";
            openFileDialog.Title = "Open a Route File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                FileSaveFormat fileData;
                try
                {
                    using (FileStream fs = (System.IO.FileStream)openFileDialog.OpenFile())
                    {
                        DataContractSerializer x = new DataContractSerializer(typeof(FileSaveFormat));
                        fileData = (FileSaveFormat)x.ReadObject(fs);
                    }
                    if (!ClearPath()) return;
                    route.Clear();
                    MapControl.ZoomLevel = fileData.zoom;
                    MapControl.MapCenter = fileData.center;
                    route = fileData.route;
                    lvRoute.ItemsSource = route;
                    foreach (TimedPosition waypoint in route)
                    {
                        AddWaypointMarkerToMap(waypoint);
                    }
                    MapControl.RegenerateRouteShape();
                    tbFileName.Text = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error while loading file: \n{0}", ex.Message), MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void NewCommendHandler(object sender, RoutedEventArgs e)
        {
            ClearPath();
            tbTime.Text = "00:00:00";
        }

        private void SaveCommandHandler(object sender, RoutedEventArgs e)
        {
            SaveToFile(tbFileName.Text);
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            DialogWindows.AboutDialog aboutDialog = new DialogWindows.AboutDialog();
            aboutDialog.Owner = this;
            aboutDialog.ShowDialog();
        }

        private void mnuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Route Files|*.route";
            saveFileDialog.Title = "Save a Route File";
            saveFileDialog.FileName = tbFileName.Text;
            saveFileDialog.ShowDialog();

            SaveToFile(saveFileDialog.FileName);
        }

        private void mnuSetApiKey_Click(object sender, RoutedEventArgs e)
        {
            DialogWindows.BingApiKeyDialog keyDialog = new DialogWindows.BingApiKeyDialog();
            keyDialog.tbKey.Text = bingApiKey;
            keyDialog.Owner = this;
            keyDialog.ShowDialog();
            if (!keyDialog.Cancelled)
            {
                bingApiKey = keyDialog.tbKey.Text;
                bingMapsClient.ApiKey = bingApiKey;

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["BingApiKey"].Value = bingApiKey;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        private void mnuOptions_Click(object sender, RoutedEventArgs e)
        {
            DialogWindows.OptionsDialog optionsDialog = new DialogWindows.OptionsDialog();
            switch (MapControl.PathColor.ToString())
            {
                case "#FF228B22":
                    optionsDialog.cmbPathColor.SelectedValue = "Green";
                    break;
                case "#FFFF0000":
                    optionsDialog.cmbPathColor.SelectedValue = "Red";
                    break;
                default:
                    optionsDialog.cmbPathColor.SelectedValue = "Blue";
                    break;
            }
            optionsDialog.tbSpeed.Text = defaultSpeed.ToString();
            optionsDialog.cbUseRealTimeTrafficData.IsChecked = useRealTimeData;
            optionsDialog.Owner = this;
            optionsDialog.ShowDialog();
            if (!optionsDialog.Cancelled)
            {
                switch ((string)optionsDialog.cmbPathColor.SelectedValue)
                {
                    case "Green":
                        MapControl.PathColor = Brushes.ForestGreen;
                        break;
                    case "Blue":
                        MapControl.PathColor = Brushes.RoyalBlue;
                        break;
                    default:
                        MapControl.PathColor = Brushes.Red;
                        break;
                }
                MapControl.RegenerateRouteShape();
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                AddOrCreateConfigSection(config, "PathColor", (string)optionsDialog.cmbPathColor.SelectedValue);
                AddOrCreateConfigSection(config, "DefaultSpeed", optionsDialog.tbSpeed.Text);
                AddOrCreateConfigSection(config, "UseRealTimeData", optionsDialog.cbUseRealTimeTrafficData.IsChecked.Value.ToString());
                defaultSpeed = Double.Parse(optionsDialog.tbSpeed.Text);
                useRealTimeData = optionsDialog.cbUseRealTimeTrafficData.IsChecked.Value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        #region Menu command helpers
        [DataContract]
        private struct FileSaveFormat
        {
            [DataMember]
            public ObservableCollection<TimedPosition> route;
            [DataMember]
            public Point center;
            [DataMember]
            public int zoom;
        }

        private void SaveToFile(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        DataContractSerializer x = new DataContractSerializer(typeof(FileSaveFormat));
                        FileSaveFormat fileData = new FileSaveFormat();
                        fileData.route = route;
                        fileData.center = MapControl.MapCenter;
                        fileData.zoom = MapControl.ZoomLevel;
                        x.WriteObject(fs, fileData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error while saving to file: \n{0}", ex.Message), MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool ClearPath()
        {
            if (route.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("This operation will clear all the current waypoints!\nWould you like to proceed?\n(This operation cannot be undone.)", MSG_BOX_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return false;
                route.Clear();
                MapControl.RemoveAllMarkers();
            }
            return true;
        }
        #endregion
        #endregion

       #region Helper methods

       private bool AddWaypointAtCurrentPosition()
        {
            double lat, lng;
            TimeSpan time;
            if (!Double.TryParse(tbPositionLat.Text, out lat) || !Double.TryParse(tbPositionLng.Text, out lng)) return false;
            if (!TimeSpan.TryParse(tbTime.Text, out time)) return false;
            // Find the right location to insert the new stop at
            int i = 0;
            for (; i < route.Count; i++)
            {
                if (route[i].Time > time) break;
                if (route[i].Time == time)
                {
                    MessageBox.Show(String.Format("Can't be in two places at the same time.\nThere's already a waypoint at time {0}.\nPlease select a different time for the new waypoint.", time), MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            TimedPosition waypoint = new TimedPosition(time, lat, lng);
            route.Insert(i, waypoint);
            AddWaypointMarkerToMap(waypoint);
            MapControl.RegenerateRouteShape();
            return true;
        }

        private void UpdateLocationUsingSearchString()
        {
            double lat, lng;
            bool found = bingMapsClient.QueryLocation(tbSearchString.Text, out lat, out lng);

            if (!found)
            {
                MessageBox.Show("Can't find: " + tbSearchString.Text, MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            tbPositionLat.Text = lat.ToString();
            tbPositionLng.Text = lng.ToString();
            MapControl.MapCenter = new Point(lat, lng);
            MapControl.SetSelectedPoint(lat, lng);
            transmittedLocation = GetTransmittedLocation(tbPositionLat.Text, tbPositionLng.Text);
        }

        private string GetTransmittedLocation(string lat, string lng)
        {
            return String.Format("{0}; {1}", lat, lng);
        }

        private void AddWaypointMarkerToMap(TimedPosition waypoint)
        {
            WaypointMarker actualMarker = new WaypointMarker(this, String.Format("Lat  : {0}\nLong : {1}", waypoint.Position.X, waypoint.Position.Y));
            MapMarker mm = MapControl.AddMarker(actualMarker, waypoint.Position.X, waypoint.Position.Y);
            waypoint.MapMarker = mm;
        }

        private void ExtendRouteFromLastPosition()
        {
            if (lvRoute.Items.Count == 0)
            {
                MessageBox.Show("Please add a start point to the route", MSG_BOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            double endLat, endLng;
            if (!Double.TryParse(tbPositionLat.Text, out endLat) || !Double.TryParse(tbPositionLng.Text, out endLng)) return;

            TimeSpan time = (lvRoute.Items[lvRoute.Items.Count - 1] as TimedPosition).Time;
            Point startPoint = (lvRoute.Items[lvRoute.Items.Count - 1] as TimedPosition).Position;

            Mouse.SetCursor(Cursors.Wait);
            
            List<TimedPosition> results;
            if (useRealTimeData) results = bingMapsClient.GetRoute(time, startPoint.X, startPoint.Y, endLat, endLng);
            else results = bingMapsClient.GetRoute(time, startPoint.X, startPoint.Y, endLat, endLng, defaultSpeed);

            Mouse.SetCursor(Cursors.Arrow);
            if (results == null) return;

            foreach (TimedPosition waypoint in results)
            {
                route.Add(waypoint);
                AddWaypointMarkerToMap(waypoint);
            }
            MapControl.RegenerateRouteShape();
        }

        #endregion

        #region IGpsEmulatorService Members

        public string  GetCurrentPosition()
        {
            clientCallDetected = true;
 	        return transmittedLocation;
        }

        #endregion
    }
}
