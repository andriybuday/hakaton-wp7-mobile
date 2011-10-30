using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using WCEmergency.Common;
using WCEmergency.WCServiceReference;

namespace WCEmergency.ViewModel
{
    public class ToiletViewModel : NotifyPropertyChanged
    {
        private WCEmergencyServiceClient WCServiceClient { get; set; }
        private GeoCoordinateWatcher Watcher { get; set; }

        public ToiletViewModel()
        {
            //ClickCommand = new RelayCommand<object>(HandleClick);

            WCServiceClient = new WCEmergencyServiceClient();
            Watcher = new GeoCoordinateWatcher();
            Watcher.PositionChanged += OnWatcherPositionChanged;
            Watcher.Start();
        }

        /* private void HandleClick(object param)
        {
            var toilet = param as Toilet;
            
            if (toilet != null)
            {
                //do something
            }
        }*/

       private void OnWatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
       {
           if (args != null || !args.Position.Location.IsUnknown)
           {
               var currentPosition = new GeoCoordinate(args.Position.Location.Latitude, args.Position.Location.Longitude,
                                                       args.Position.Location.Altitude);

               WCServiceClient.GetNearestToiltesCompleted += OnGetNearestToiltesCompleted;
               WCServiceClient.GetNearestToiltesAsync(args.Position.Location, 0);
           }
       }

        private void OnGetNearestToiltesCompleted(object sender, GetNearestToiltesCompletedEventArgs e)
        {
            Items = e.Result;
            WCServiceClient.GetNearestToiltesCompleted -= OnGetNearestToiltesCompleted;
            Watcher.Stop();
        }

        private ObservableCollection<Toilet> _items;
        public ObservableCollection<Toilet> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }
    }
}
