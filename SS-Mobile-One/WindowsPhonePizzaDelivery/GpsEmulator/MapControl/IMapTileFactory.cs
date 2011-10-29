using GpsEmulator.Utilities;

namespace GpsEmulator.MapControl
{
    public interface IMapTileFactory
    {
        MapTile GetTile(int zoom, int tileX, int tileY, MapType type);
    }
}
