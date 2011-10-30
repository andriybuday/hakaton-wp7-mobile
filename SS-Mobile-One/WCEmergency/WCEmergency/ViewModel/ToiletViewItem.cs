using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using WCEmergency.Common;
using WCEmergency.View;
using WCEmergency.WCServiceReference;
using System.Windows.Input;

namespace WCEmergency.ViewModel
{
    public class ToiletViewItem : Toilet
    {
        public ICommand ClickCommand { get; set; }

        public double Distance { get; set; }

        public ObservableCollection<ToiletViewItem> Items;

        public string Color { get; set; }

        public string Text { get; set; }

        public ToiletViewItem(Toilet toilet, ObservableCollection<ToiletViewItem> items, GeoCoordinate currentPosition)
        {
            Id = toilet.Id;
            Name = toilet.Name;
            Description = toilet.Description;
            Rate = toilet.Rate;
            Coordinate = toilet.Coordinate;
            Picture = toilet.Picture;
            Sex = toilet.Sex;
            ClickCommand = new RelayCommand<object>(HandleClick);
            Distance = CalculateDistance(Coordinate, currentPosition);
            Items = items;
            Text = "WC";
            Color = "Green";
        }

        private double CalculateDistance(GeoCoordinate coordinate, GeoCoordinate currentPosition)
        {
            if (coordinate == null) return 0;
            return Math.Round(currentPosition.GetDistanceTo(coordinate), 0);
        }

        private void HandleClick(object param)
        {
            var toilet = param as ToiletViewItem;

            if (toilet != null)
            {
                ToiletContext.Instance.Item = toilet;
                OnClickEvent(toilet);
            }
        }

        public event EventHandler<NavigationEventArgs> ClickEvent;

        private void OnClickEvent(ToiletViewItem toilet)
        {
            if (ClickEvent != null)
            {
                ClickEvent(this, new NavigationEventArgs(toilet, new Uri(Constants.MapView, UriKind.Relative)));
            }
        }
    }
}
