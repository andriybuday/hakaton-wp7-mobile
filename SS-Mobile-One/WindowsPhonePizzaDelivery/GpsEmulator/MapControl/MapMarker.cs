using System.Windows;
using System.Windows.Controls;
using GpsEmulator.Utilities;

namespace GpsEmulator.MapControl
{
    public class MapMarker
    {
        public Point Location;
        public UserControl MarkerControl;

        public MapMarker(Point location, UserControl markerControl)
        {
            Location = location;
            MarkerControl = markerControl;
        }

        public void SetVisualLocation(int zoomLevel)
        {
            int x, y;
            MapUtils.LatLongToPixelXY(Location.X, Location.Y, zoomLevel, out x, out y);
            MarkerControl.Margin = new Thickness(x-MarkerControl.Width/2, y-MarkerControl.Height/2, 0, 0);
        }
    }
}
