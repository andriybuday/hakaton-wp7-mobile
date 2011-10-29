using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using GpsEmulator.Utilities;

namespace GpsEmulator.Markers
{
    public class RouteMarker : UserControl
    {
        GpsEmulator.MapControl.MapControl mapControl;

        public RouteMarker(GpsEmulator.MapControl.MapControl mapControl)
        {
            this.mapControl = mapControl;
            this.Width = 1;
            this.Height = 1;
        }

    }
}
