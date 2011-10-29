using System;
using System.Windows.Media.Imaging;
using GpsEmulator.BingApis;
using GpsEmulator.Utilities;

namespace GpsEmulator.MapControl
{
    public class WebMapTileFactory : IMapTileFactory
    {
        BingMapsClient bingMapsClient;

        public WebMapTileFactory(BingMapsClient bingMapsClient)
        {
            this.bingMapsClient = bingMapsClient;
        }

        public MapTile GetTile(int zoom, int tileX, int tileY, MapType type)
        {
            MapTile tile = new MapTile();
            try
            {
                BitmapImage img = bingMapsClient.GetTile(zoom, tileX, tileY, type);
                img.Changed += ((a, b) => { tile.SetImage(img); });
            }
            catch (Exception ex)
            {
                tile.SetDescription(ex.Message);
            }
            return tile;
        }
    }
}
