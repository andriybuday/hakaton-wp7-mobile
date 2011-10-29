using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GpsEmulator.Utilities;

namespace GpsEmulator.Markers
{
    /// <summary>
    /// Interaction logic for WaypointMarker.xaml
    /// </summary>
    public partial class WaypointMarker : UserControl
    {
        Popup Popup;
        Label Label;
        MainWindow MainWindow;

        public WaypointMarker(MainWindow window, string title)
        {
            this.InitializeComponent();

            this.MainWindow = window;

            Popup = new Popup();
            Popup.SetValue(Canvas.ZIndexProperty, 900);
            Label = new Label();

            this.Loaded += new RoutedEventHandler(WaypointMarker_Loaded);
            this.MouseEnter += new MouseEventHandler(WaypointMarker_MouseEnter);
            this.MouseLeave += new MouseEventHandler(WaypointMarker_MouseLeave);

            Popup.Placement = PlacementMode.Mouse;
            Label.Background = Brushes.Blue;
            Label.Foreground = Brushes.White;
            Label.BorderBrush = Brushes.WhiteSmoke;
            Label.BorderThickness = new Thickness(1);
            Label.Padding = new Thickness(5);
            Label.FontSize = 14;
            Label.Content = title;
            Popup.Child = Label;
        }

        void WaypointMarker_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void WaypointMarker_MouseLeave(object sender, MouseEventArgs e)
        {
            Popup.SetValue(Canvas.ZIndexProperty, 900);
            Popup.IsOpen = false;
        }

        void WaypointMarker_MouseEnter(object sender, MouseEventArgs e)
        {
            Popup.SetValue(Canvas.ZIndexProperty, 9000);
            Popup.IsOpen = true;
        }
    }
}
