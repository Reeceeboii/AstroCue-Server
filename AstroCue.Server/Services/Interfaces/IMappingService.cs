namespace AstroCue.Server.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Misc;

    public interface IMappingService
    {
        /// <summary>
        /// Proxy around the MapBox forward geocoding endpoint: https://docs.mapbox.com/api/search/geocoding/.
        /// --
        /// "Forward geocoding converts location text into geographic coordinates,
        /// turning 2 Lincoln Memorial Circle NW into -77.050,38.889."
        /// </summary>
        /// <param name="query">The string query coming from a client. This is validated against MapBox's limits</param>
        /// <returns>A list of <see cref="FwdGeocodeResult"/> instances</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the query is out of MapBox's expected range of words/chars</exception>
        /// <exception cref="ArgumentException">If the query contains any illegal characters (i.e. a semicolon)</exception>
        Task<IList<FwdGeocodeResult>> ForwardGeocodeAsync(string query);

        /// <summary>
        /// Get a static map image from the MapBox API
        /// </summary>
        /// <param name="longitude">The longitude of the map</param>
        /// <param name="latitude">The latitude of the map</param>
        /// <returns>A byte array representing the contents of the image</returns>
        Task<byte[]> GetStaticMapImageAsync(float longitude, float latitude);
    }
}