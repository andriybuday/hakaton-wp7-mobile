using WCEmergency.Bing.Geocode;

namespace WCEmergency.Helpers
{
    /// <summary>
    /// Internally used for passing state between route asynchronous calls.
    /// </summary>
    internal class RoutingState
    {
        internal RoutingState(GeocodeResult[] resultArray, int index, string tb)
        {
            Results = resultArray;
            LocationNumber = index;
            Output = tb;
        }

        internal bool GeocodesComplete
        {
            get
            {
                for (int idx = 0; idx < Results.Length; idx++)
                {
                    if (null == Results[idx])
                        return false;
                }
                return true;
            }
        }

        internal bool GeocodesSuccessful
        {
            get
            {
                for (int idx = 0; idx < Results.Length; idx++)
                {
                    if (null == Results[idx] || null == Results[idx].Locations || 0 == Results[idx].Locations.Count)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal GeocodeResult[] Results { get; set; }
        internal int LocationNumber { get; set; }
        internal string Output { get; set; }
    }
}
