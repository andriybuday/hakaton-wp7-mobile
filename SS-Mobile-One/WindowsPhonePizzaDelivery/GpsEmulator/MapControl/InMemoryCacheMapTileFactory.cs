using System;
using System.Collections.Generic;
using System.Threading;
using GpsEmulator.Utilities;
using System.Linq;

namespace GpsEmulator.MapControl
{
    class InMemoryCacheMapTileFactory : IMapTileFactory
    {
        Dictionary<string, MapTile> cache;
        Dictionary<string, long> cacheAccessCounter;
        ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        IMapTileFactory innerFactory;

        public InMemoryCacheMapTileFactory(IMapTileFactory innerFactory)
        {
            this.innerFactory = innerFactory;
            cache = new Dictionary<string, MapTile>();
            cacheAccessCounter = new Dictionary<string, long>();
        }

        public MapTile GetTile(int zoom, int tileX, int tileY, MapType type)
        {
            string key = String.Format("{0},{1},{2},{3}", zoom, tileX, tileY, type);
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (cache.ContainsKey(key))
                {
                    cacheAccessCounter[key] = DateTime.Now.Ticks;
                    return cache[key];
                }
                else
                {
                    MapTile tile = innerFactory.GetTile(zoom, tileX, tileY, type);
                    cacheLock.EnterWriteLock();
                    try
                    {
                        if (tile != null)
                        {
                            cache[key] = tile;
                            cacheAccessCounter[key] = DateTime.Now.Ticks;
                        }
                        return tile;
                    }
                    finally
                    {
                        cacheLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Cleans the cache, keeping only the 1000 tiles that were accessed most recently.
        /// </summary>
        public void CleanupCache()
        {
            List<string> deletionList = new List<string>();
            cacheLock.EnterWriteLock();
            try
            {
                var tmp = from string key in cacheAccessCounter.Keys
                        orderby cacheAccessCounter[key] descending
                        select key;
                var keys = tmp.ToArray();
                if (keys.Length < 500) return;

                for (int i = 500; i < keys.Length; i++)
                {
                    cacheAccessCounter.Remove(keys[i]);
                    cache.Remove(keys[i]);
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

        }
    }
}
