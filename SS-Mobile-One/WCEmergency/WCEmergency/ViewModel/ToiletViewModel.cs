using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using WCEmergency.Common;
using WCEmergency.View;
using WCEmergency.WCServiceReference;

namespace WCEmergency.ViewModel
{
    public class ToiletViewModel : NotifyPropertyChanged
    {
        public ICommand ClickCommand { get; set; }

        private WCEmergencyServiceClient WCServiceClient { get; set; }
        private GeoCoordinateWatcher Watcher { get; set; }
 
        public ToiletViewModel()
        {
            WCServiceClient = new WCEmergencyServiceClient();
            Watcher = new GeoCoordinateWatcher();
            Watcher.PositionChanged += OnWatcherPositionChanged;
            Watcher.Start();
        }

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
            var list = e.Result;

            if (Items != null && Items.Count == 0)
            {
                foreach (var toilet in list)
                {
                    var toiletView = new ToiletViewItem(toilet, Items);
                    toiletView.ClickEvent += OnToiletChoosen;
                    Items.Add(toiletView);
                }
            }

            WCServiceClient.GetNearestToiltesCompleted -= OnGetNearestToiltesCompleted;
            Watcher.Stop();
        }


        private ObservableCollection<ToiletViewItem> _items = new ObservableCollection<ToiletViewItem>();
        public ObservableCollection<ToiletViewItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }
        
        public event EventHandler<NavigationEventArgs> ToiletChoosen;

        private void OnToiletChoosen(object sender, NavigationEventArgs navigationEventArgs)
        {
            if (ToiletChoosen != null)
            {
                var emptyItems = ((ToiletViewItem)navigationEventArgs.Content).Items.Where(x => string.IsNullOrEmpty(x.Name)).ToList<ToiletViewItem>();

                foreach (ToiletViewItem item in emptyItems)
                {
                    Items.Remove(item);
                }

                ToiletChoosen(this, navigationEventArgs);
            }
        }
    }
}
