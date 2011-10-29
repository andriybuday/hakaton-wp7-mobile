using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GpsEmulator.MapControl
{
    /// <summary>
    /// Interaction logic for MapTile.xaml
    /// </summary>
    public partial class MapTile : UserControl
    {
        public BitmapImage Image;

        public MapTile()
        {
            InitializeComponent();
        }

        public void SetImage(BitmapImage image)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                imgTileImage.Source = image;
            }), DispatcherPriority.Render);
        }

        public void SetDescription(string description)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tbDescription.Text = description;
                }), DispatcherPriority.Render);
        }
    }
}
