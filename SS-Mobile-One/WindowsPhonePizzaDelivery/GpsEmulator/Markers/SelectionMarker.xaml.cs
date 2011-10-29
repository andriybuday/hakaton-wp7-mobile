using System.Windows.Controls;

namespace GpsEmulator.Markers
{
    /// <summary>
    /// Interaction logic for PositionMarker.xaml
    /// </summary>
    public partial class SelectionMarker : UserControl
    {
        public SelectionMarker()
        {
            InitializeComponent();
            SetValue(Canvas.ZIndexProperty, 300);
        }

    }
}
