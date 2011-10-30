using System;
using WCEmergency.Bing.Geocode;
using WCEmergency.Bing.Route;

namespace WCEmergency.Helpers
{
    /// <summary>
    /// Represents a route calculation error.
    /// </summary>
    public class RouteCalculationError
    {
        private const string NoResults = "No results.";        

        /// <summary>
        /// Gets the reason of the error.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Get the exception instance or null if none.
        /// </summary>
        public Exception Exception { get; private set; }

        internal RouteCalculationError(string reason, Exception exception)
        {
            Reason = reason;
            Exception = exception;
        }

        internal RouteCalculationError(GeocodeCompletedEventArgs e)
        {
            if (e.Result == null ||
                e.Result.ResponseSummary == null ||
                string.IsNullOrEmpty(e.Result.ResponseSummary.FaultReason))
            {
                Reason = NoResults;
            }
            else
            {
                Reason = e.Result.ResponseSummary.FaultReason;
            }

            Exception = e.Error;
        }

        internal RouteCalculationError(CalculateRouteCompletedEventArgs e)
        {
            if (e.Result == null ||
                e.Result.ResponseSummary == null ||
                string.IsNullOrEmpty(e.Result.ResponseSummary.FaultReason))
            {
                Reason = NoResults;
            }
            else
            {
                Reason = e.Result.ResponseSummary.FaultReason;
            }

            Exception = e.Error;
        }
    }
}
