using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using PizzaDeliveryServices.BingMapsRouteService;
using GeocodeLocation = PizzaDeliveryServices.BingMapsGeocodeService.GeocodeLocation;
using GeocodeService = PizzaDeliveryServices.BingMapsGeocodeService;
using RouteService = PizzaDeliveryServices.BingMapsRouteService;

namespace PizzaDeliveryServices
{
    public interface IBingMapsServices
    {
        CalculateMileageResult CalculateMileage(CalculateMileageRequest request);
        CalculateRouteResult CalculateRoute(CalculateRouteRequest request);
    }

    class BingMapsServices
    {
        private readonly string _bingMapsKey;

        public BingMapsServices()
        {
            _bingMapsKey = ConfigurationManager.AppSettings["Bing_Services_Key"];
        }

        public GeocodeLocation GetCoordinates(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return null;
            }

            GeocodeService.GeocodeLocation resultLocation = null;

            GeocodeService.GeocodeRequest geocodeRequest = new GeocodeService.GeocodeRequest();

            // Set the credentials using a valid Bing Maps key
            geocodeRequest.Credentials = new GeocodeService.Credentials();
            geocodeRequest.Credentials.ApplicationId = _bingMapsKey;

            // Set the full address query
            geocodeRequest.Query = address;

            // Set the options to only return high confidence results 
            GeocodeService.ConfidenceFilter[] filters = new GeocodeService.ConfidenceFilter[1];
            filters[0] = new GeocodeService.ConfidenceFilter();
            filters[0].MinimumConfidence = GeocodeService.Confidence.Medium;

            // Add the filters to the options
            GeocodeService.GeocodeOptions geocodeOptions = new GeocodeService.GeocodeOptions();
            geocodeOptions.Filters = filters;
            geocodeRequest.Options = geocodeOptions;

            // Make the geocode request
            GeocodeService.GeocodeServiceClient geocodeService = new GeocodeService.GeocodeServiceClient();
            GeocodeService.GeocodeResponse geocodeResponse = geocodeService.Geocode(geocodeRequest);


            if (geocodeResponse.Results.Length > 0)
            {
                resultLocation = geocodeResponse.Results[0].Locations[0];
            }

            return resultLocation;
        }

        private RouteService.RouteResponse GetRouteResponse(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
        {
            RouteService.RouteRequest routeRequest = new RouteService.RouteRequest();

            // Set the credentials using a valid Bing Maps key
            routeRequest.Credentials = new RouteService.Credentials();
            routeRequest.Credentials.ApplicationId = _bingMapsKey;

            RouteService.Waypoint[] waypoints = new RouteService.Waypoint[2];

            //start waypoint
            waypoints[0] = new RouteService.Waypoint
            {
                Description = "Start",
                Location = new RouteService.Location
                {
                    Latitude = startLatitude,
                    Longitude = startLongitude
                }
            };
            //end waypoint
            waypoints[1] = new RouteService.Waypoint
            {
                Description = "End",
                Location = new RouteService.Location
                {
                    Latitude = endLatitude,
                    Longitude = endLongitude
                }
            };

            routeRequest.Waypoints = waypoints;
            routeRequest.Culture = "en-US";
            routeRequest.UserProfile = new RouteService.UserProfile { DistanceUnit = RouteService.DistanceUnit.Mile };

            // Make the calculate route request
            var routeService = new RouteService.RouteServiceClient();
            return routeService.CalculateRoute(routeRequest);
        }

        private double GetMileage(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
        {
            var routeResponse = GetRouteResponse(startLatitude, startLongitude, endLatitude, endLongitude);

            if (routeResponse != null && routeResponse.Result.Legs.Length > 0)
            {
                //get the distance
                return routeResponse.Result.Summary.Distance;
            }
            else
                throw new ApplicationException("Failed to calculate mileage");
        }

        private IList<RouteDirectionModel> GetDirections(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
        {
            var routeResponse = GetRouteResponse(startLatitude, startLongitude, endLatitude, endLongitude);
            var directionsList = new List<RouteDirectionModel>();

            if (routeResponse != null && routeResponse.Result.Legs.Length > 0)
            {
                //get the directions
                int instructionCount = 0;

                var regex = new Regex("<[/a-zA-Z:]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                foreach (RouteService.RouteLeg leg in routeResponse.Result.Legs)
                {
                    foreach (RouteService.ItineraryItem item in leg.Itinerary)
                    {
                        instructionCount++;
                        var directionDescription = regex.Replace(item.Text, string.Empty);
                        directionsList.Add(new RouteDirectionModel(directionDescription, instructionCount, item.Summary.Distance));
                    }
                }
                return directionsList;
            }
            else
                throw new ApplicationException("Failed to get route directions");
        }

        public CalculateMileageResult CalculateMileage(CalculateMileageRequest request)
        {
            Console.WriteLine("About to calculate mileage");
            //get start coortinates; we always try first address and if it fails we try with GPS
            double[] startCoords;
            startCoords = GetStartCoordsFirstAddress(request.StartAddress, request.StartLatitude, request.StartLongitude);

            //get end coordinates; we always try to calculate this basing on address; we are not trying to get them by GPS coords
            var endGeocodeLocation = GetCoordinates(request.EndAddress);
            if (endGeocodeLocation == null)
            {
                throw new ApplicationException("Failed to get coordinates for second visit");
            }

            //get the mileage
            var mileage = GetMileage(startCoords[0], startCoords[1], endGeocodeLocation.Latitude, endGeocodeLocation.Longitude);
            return new CalculateMileageResult(true, "OK") { Mileage = mileage };
        }

        public CalculateRouteResult CalculateRoute(CalculateRouteRequest request)
        {
            Console.WriteLine(string.Format("About to calculate route, with {0}StartAddress:'{1}'{0}Lng:{2},Lat:{3}{0}EndAddress:'{4}'{0}"
            , Environment.NewLine, request.StartAddress, request.StartLongitude, request.StartLatitude, request.EndAddress));

            double[] startCoords;
            var startGeocodeLocation = GetCoordinates(request.StartAddress);
            if (request.StartLatitude != null && request.StartLongitude != null)
            {
                startCoords = new[] { request.StartLatitude.Value, request.StartLongitude.Value };
            }
            else if (startGeocodeLocation != null)
            {
                startCoords = new[] { startGeocodeLocation.Latitude, startGeocodeLocation.Longitude };
            }
            else
            {
                throw new ApplicationException("Failed to get coordinates for first visit");
            }


            //get end coordinates; we always try to calculate this basing on address; we are not trying to get them by GPS coords
            var endGeocodeLocation = GetCoordinates(request.EndAddress);
            if (endGeocodeLocation == null)
            {
                throw new ApplicationException("Failed to get coordinates for second visit");
            }

            //get the mileage
            var result = GetDirections(startCoords[0], startCoords[1], endGeocodeLocation.Latitude, endGeocodeLocation.Longitude);
            return new CalculateRouteResult(true, "OK", result);
        }

        private double[] GetStartCoordsFirstAddress(string startAddress, double? startLatitude, double? startLongitude)
        {
            double[] startCoords;
            var startGeocodeLocation = GetCoordinates(startAddress);
            if (startGeocodeLocation != null)
            {
                startCoords = new[] { startGeocodeLocation.Latitude, startGeocodeLocation.Longitude };
            }
            else if (startLatitude != null && startLongitude != null)
            {
                startCoords = new[] { startLatitude.Value, startLongitude.Value };
            }
            else
            {
                throw new ApplicationException("Failed to get coordinates for first visit");
            }
            return startCoords;
        }
    }
}
