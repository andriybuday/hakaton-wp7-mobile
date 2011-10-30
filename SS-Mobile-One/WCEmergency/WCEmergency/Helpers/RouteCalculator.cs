

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Threading;

using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using WCEmergency.Bing.Geocode;
using WCEmergency.Bing.Route;

namespace WCEmergency.Helpers
{
    /// <summary>
    /// Helper class for simplifying the process of calculating a route asynchronously,
    /// using Bing maps web services.
    /// </summary>
    public sealed class RouteCalculator
    {
        #region Fields
        private readonly object StateSync = new object();
        private readonly CredentialsProvider _credentialsProvider;
        private readonly string _to;
        private readonly string _from;
        private readonly Dispatcher _uiDispatcher;
        private readonly Action<RouteResponse> _routeFound;
        private readonly GeocodeServiceClient _geocodeClient;
        private readonly RouteServiceClient _routeClient;
        private bool _geoFailed = false;
        #endregion

        #region Events

        public event Action<RouteCalculationError> Error = delegate { };

        #endregion

        #region Ctor
        public RouteCalculator(
            CredentialsProvider credentialsProvider,
            string to,
            string from,
            Dispatcher uiDispatcher,
            Action<RouteResponse> routeFound)
        {
            if (credentialsProvider == null)
            {
                throw new ArgumentNullException("credentialsProvider");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("to");
            }

            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentNullException("from");
            }

            if (uiDispatcher == null)
            {
                throw new ArgumentNullException("uiDispatcher");
            }

            if (routeFound == null)
            {
                throw new ArgumentNullException("routeFound");
            }

            _credentialsProvider = credentialsProvider;
            _to = to;
            _from = from;
            _uiDispatcher = uiDispatcher;
            _routeFound = routeFound;

            _geocodeClient = new GeocodeServiceClient();
            _geocodeClient.GeocodeCompleted += client_GeocodeCompleted;

            _routeClient = new RouteServiceClient();
            _routeClient.CalculateRouteCompleted += client_RouteCompleted;
        }

        #endregion

        #region Operations
        public void CalculateAsync()
        {
            // Geocode locations in parallel.
            var results = new GeocodeResult[2];

            // To location.
            var state1 = new RoutingState(results, 1, _to);
            GeocodeAddress(_to, state1);

            // From location.
            var state0 = new RoutingState(results, 0, _from);
            GeocodeAddress(_from, state0);
        }
        #endregion

        #region Privates
        private void client_GeocodeCompleted(object sender, GeocodeCompletedEventArgs e)
        {
            try
            {
                RoutingState state = e.UserState as RoutingState;
                GeocodeResult result = null;

                lock (StateSync)
                {
                    if (_geoFailed)
                        return;
                }

                if (e.Result.ResponseSummary.StatusCode != Bing.Geocode.ResponseStatusCode.Success ||
                    e.Result.Results.Count == 0)
                {
                    lock (StateSync)
                    {
                        _geoFailed = true;
                    }

                    // Report geocode error.
                    _uiDispatcher.BeginInvoke(() => Error(new RouteCalculationError(e)));

                    return;
                }

                bool doneGeocoding = false;

                lock (StateSync)
                {
                    // Only report on first result.
                    result = e.Result.Results.First();

                    // Update state object ... when all the results are set, call route.
                    state.Results[state.LocationNumber] = result;
                    doneGeocoding = state.GeocodesComplete;
                }

                if (doneGeocoding && state.GeocodesSuccessful)
                {
                    // Calculate the route.
                    CalculateRoute(state.Results);
                }
            }
            catch (Exception ex)
            {
                lock (StateSync)
                {
                    _geoFailed = true;
                }

                _uiDispatcher.BeginInvoke(() => Error(new RouteCalculationError(ex.Message, ex)));
            }
        }

        private void client_RouteCompleted(object sender, CalculateRouteCompletedEventArgs e)
        {
            if (e.Result.ResponseSummary.StatusCode == Bing.Route.ResponseStatusCode.Success)
            {
                // Raise the found event on the UI thread.
                _uiDispatcher.BeginInvoke(() => _routeFound(e.Result));
            }
            else
            {
                // Report route error.
                _uiDispatcher.BeginInvoke(() => Error(new RouteCalculationError(e)));
            }
        }

        /// <summary>
        /// Calculates a route, based on geocode resuls.
        /// </summary>
        private void CalculateRoute(GeocodeResult[] locations)
        {
            // Preparing a request for route calculation.
            var request = new RouteRequest()
            {
                Culture = CultureInfo.CurrentUICulture.Name,
                Waypoints = new ObservableCollection<Waypoint>(),

                // Don't raise exceptions.
                ExecutionOptions = new Bing.Route.ExecutionOptions()
                {
                    SuppressFaults = true
                },

                // Only accept results with high confidence.
                Options = new RouteOptions()
                {
                    RoutePathType = RoutePathType.Points
                }
            };

            foreach (var result in locations)
            {
                request.Waypoints.Add(GeocodeResultToWaypoint(result));
            }

            // Get credentials and only then place an async call on the route service.
            _credentialsProvider.GetCredentials(credentials =>
            {
                // Pass in credentials for web services call.
                // Replace with your own Credentials.
                request.Credentials = credentials;

                // Make asynchronous call to fetch the data.
                _routeClient.CalculateRouteAsync(request);
            });
        }

        private Waypoint GeocodeResultToWaypoint(GeocodeResult result)
        {
            return new Waypoint()
            {
                Description = result.DisplayName,
                Location = new Location()
                {
                    Latitude = result.Locations[0].Latitude,
                    Longitude = result.Locations[0].Longitude
                }
            };
        }

        private void GeocodeAddress(string address, RoutingState state)
        {
            var request = new GeocodeRequest()
            {
                Culture = CultureInfo.CurrentUICulture.Name,
                Query = address,

                // Don't raise exceptions.
                ExecutionOptions = new Bing.Geocode.ExecutionOptions()
                {
                    SuppressFaults = true
                },

                // Only accept results with high confidence.
                Options = new GeocodeOptions()
                {
                    Filters = new ObservableCollection<FilterBase>
                    {
                        new ConfidenceFilter()
                        {
                            MinimumConfidence = Bing.Geocode.Confidence.High
                        }
                    }
                }
            };

            // Get credentials and only then place an async call on the geocode service.
            _credentialsProvider.GetCredentials(credentials =>
            {
                // Pass in credentials for web services call.
                // Replace with your own Credentials.
                request.Credentials = credentials;

                // Make asynchronous call to fetch the data.
                _geocodeClient.GeocodeAsync(request, state);
            });
        }
        #endregion        
    }
}
