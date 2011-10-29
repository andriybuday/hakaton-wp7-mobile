using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using dev.virtualearth.net.webservices.v1.common;
using dev.virtualearth.net.webservices.v1.geocode;
using dev.virtualearth.net.webservices.v1.route;
using GpsEmulator.Utilities;

namespace GpsEmulator.BingApis
{
    public class BingMapsClient
    {
        Credentials creds = new Credentials();
        RouteServiceClient routeService = new RouteServiceClient("CustomBinding_IRouteService");
        ImageryServiceClient imageryService = new ImageryServiceClient("CustomBinding_IImageryService");

        #region Properties

        string apiKey = null;
        public string ApiKey
        {
            get { return apiKey; }
            set
            {
                apiKey = value;
                creds.ApplicationId = apiKey;
            }
        }

        public int MaxServiceRetries = 5;

        #endregion

        public BingMapsClient(string bingApiKey)
        {
            creds.ApplicationId = bingApiKey;
        }

        public bool QueryLocation(string queryString, out double lat, out double lng)
        {
            lat = 0;
            lng = 0;

            GeocodeRequest geocodeRequest = new GeocodeRequest();
            geocodeRequest.Query = queryString;
            geocodeRequest.Options = new GeocodeOptions() { Filters = new FilterBase[] { new ConfidenceFilter() { MinimumConfidence = Confidence.High } }, Count = 1 };
            geocodeRequest.Credentials = creds;

            GeocodeServiceClient geocodeServiceClient = new GeocodeServiceClient("CustomBinding_IGeocodeService");
            GeocodeResponse geocodeResponse = geocodeServiceClient.Geocode(geocodeRequest);

            bool locationFound = geocodeResponse.Results.Count() > 0;
            if (locationFound)
            {
                lat = geocodeResponse.Results[0].Locations[0].Latitude;
                lng = geocodeResponse.Results[0].Locations[0].Longitude;
            }
            return locationFound;
        }

        public List<TimedPosition> GetRoute(TimeSpan startTime, double startLat, double startLong, double endLat, double endLong)
        {
            RouteRequest routeRequest = new RouteRequest();
            routeRequest.Credentials = creds;
            routeRequest.Options = new RouteOptions() { RoutePathType = RoutePathType.Points };

            Waypoint wp1 = new Waypoint()
            {
                Description = "Start",
                Location = new Location() { Latitude = startLat, Longitude = startLong }
            };
            Waypoint wp2 = new Waypoint()
            {
                Description = "End",
                Location = new Location() { Latitude = endLat, Longitude = endLong }
            };
            routeRequest.Waypoints = new Waypoint[] { wp1, wp2 };

            // Make the calculate route request
            int attempts = 0;
            RouteResponse routeResponse = null;
            while (routeResponse == null && attempts < MaxServiceRetries)
            {
                try
                {
                    routeResponse = routeService.CalculateRoute(routeRequest);
                }
                finally
                {
                    attempts++;
                }
            }
            if (attempts == MaxServiceRetries | routeResponse.Result.RoutePath.Points.Length < 2) return null;

            List<TimedPosition> path = new List<TimedPosition>();

            for (int i = 0; i < routeResponse.Result.RoutePath.Points.Length; i++)
            {
                if (i > 0)
                {
                    routeRequest.Waypoints = new Waypoint[] {
                        new Waypoint() {
                            Location = new Location() {
                                Longitude = routeResponse.Result.RoutePath.Points[i-1].Longitude,
                                Latitude= routeResponse.Result.RoutePath.Points[i-1].Latitude
                            }
                        }, 
                        new Waypoint() {
                            Location = new Location() {
                                Longitude = routeResponse.Result.RoutePath.Points[i].Longitude,
                                Latitude= routeResponse.Result.RoutePath.Points[i].Latitude
                            }
                        }
                    };
                    RouteResponse subRouteResponse = routeService.CalculateRoute(routeRequest);
                    startTime = startTime.Add(TimeSpan.FromSeconds(subRouteResponse.Result.Summary.TimeInSeconds));
                }
                path.Add(new TimedPosition(startTime, routeResponse.Result.RoutePath.Points[i].Latitude, routeResponse.Result.RoutePath.Points[i].Longitude));
            }

            return path;
        }

        public List<TimedPosition> GetRoute(TimeSpan startTime, double startLat, double startLong, double endLat, double endLong, double speedOfTravel)
        {
            RouteRequest routeRequest = new RouteRequest();
            routeRequest.Credentials = creds;
            routeRequest.Options = new RouteOptions() { RoutePathType = RoutePathType.Points };

            Waypoint wp1 = new Waypoint()
            {
                Description = "Start",
                Location = new Location() { Latitude = startLat, Longitude = startLong }
            };
            Waypoint wp2 = new Waypoint()
            {
                Description = "End",
                Location = new Location() { Latitude = endLat, Longitude = endLong }
            };
            routeRequest.Waypoints = new Waypoint[] { wp1, wp2 };

            // Make the calculate route request
            int attempts = 0;
            RouteResponse routeResponse = null;
            while (routeResponse == null && attempts < MaxServiceRetries)
            {
                try
                {
                    routeResponse = routeService.CalculateRoute(routeRequest);
                }
                finally
                {
                    attempts++;
                }
            }
            if (attempts == MaxServiceRetries | routeResponse.Result.RoutePath.Points.Length <2) return null;

            List<TimedPosition> path = new List<TimedPosition>(routeResponse.Result.RoutePath.Points.Length);
            System.Windows.Point startPoint, endPoint;

            startPoint = new System.Windows.Point(
                routeResponse.Result.RoutePath.Points[0].Longitude,
                routeResponse.Result.RoutePath.Points[0].Latitude);
            TimeSpan time = startTime;

            for (int i = 1; i < routeResponse.Result.RoutePath.Points.Length; i++)
            {
                endPoint = new System.Windows.Point(
                    routeResponse.Result.RoutePath.Points[i].Longitude,
                    routeResponse.Result.RoutePath.Points[i].Latitude);
                double distance = MapUtils.GetDistance(startPoint, endPoint);
                time = time.Add(TimeSpan.FromMilliseconds(100 * distance / speedOfTravel));
                path.Add(new TimedPosition(time, routeResponse.Result.RoutePath.Points[i].Latitude, routeResponse.Result.RoutePath.Points[i].Longitude));
                startPoint = endPoint;
            }
            return path;
        }

        internal BitmapImage GetTile(int zoom, int tileX, int tileY, MapType type)
        {
            string tileUrl = GetTileUrl(zoom, tileX, tileY, type);
            BitmapImage image = new BitmapImage();

            // Make the image request on a different thread, but create the actual image on the current one
            Task.Factory.StartNew<MemoryStream>(delegate()
            {
                WebClient client = new WebClient();
                int attempts = 0;
                byte[] tileBytes = null;
                while (attempts < MaxServiceRetries)
                {
                    try
                    {
                        tileBytes = client.DownloadData(tileUrl);
                        return new MemoryStream(tileBytes);
                    }
                    catch
                    {
                        attempts++;
                    }
                }
                return null;
            })
            .ContinueWith((t) =>
            {
                if (!t.IsFaulted && t.Result!=null)
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = t.Result;
                    image.EndInit();
                    t.Result.Close();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            return image;
        }

        internal string GetTileUrl(int zoom, int tileX, int tileY, MapType type, string language="en-US")
        {
            const string VersionBingMaps = "517";

            string key = MapUtils.TileXYToQuadKey(tileX, tileY, zoom);

            int serverNum = ((int) (tileX + 2 * tileY)) % 4;

            switch (type)
            {
                case MapType.Road:
                    {
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}.png?g={2}&mkt={3}{4}", serverNum, key, VersionBingMaps, language, (!string.IsNullOrEmpty(creds.ApplicationId) ? "&token=" + creds.ApplicationId : string.Empty));
                        // If you want lower image quality, change this to: return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}.png?g={2}&mkt={3}{4}", serverNum, key, VersionBingMaps, language, (!string.IsNullOrEmpty(creds.ApplicationId) ? "&token=" + creds.ApplicationId : string.Empty));
                    }

                case MapType.Aerial:
                    {
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/a{1}.jpeg?g={2}&mkt={3}{4}", serverNum, key, VersionBingMaps, language, (!string.IsNullOrEmpty(creds.ApplicationId) ? "&token=" + creds.ApplicationId : string.Empty));
                    }

                case MapType.Hybrid:
                    {
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/h{1}.jpeg?g={2}&mkt={3}{4}", serverNum, key, VersionBingMaps, language, (!string.IsNullOrEmpty(creds.ApplicationId) ? "&token=" + creds.ApplicationId : string.Empty));
                    }
            }

            return null;
        }
    }
}