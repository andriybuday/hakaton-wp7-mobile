using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GpsEmulator.Markers
{
    /// <summary>
    /// Interaction logic for PositionMarker.xaml
    /// </summary>
    public partial class DirectionalMarker : UserControl
    {
        RotateTransform rotateTransform = new RotateTransform(0);

        public DirectionalMarker()
        {
            InitializeComponent();
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = rotateTransform;
            SetValue(Canvas.ZIndexProperty, 300);
        }

        public void SetHeading(double x, double y)
        {
            rotateTransform.Angle = Math.Atan2(y,x) * 180 / Math.PI ;
        }
    }
}
