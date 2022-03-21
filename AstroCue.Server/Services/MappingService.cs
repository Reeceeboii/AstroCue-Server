namespace AstroCue.Server.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Data.Interfaces;
    using Interfaces;
    using Models.Misc;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    /// <summary>
    /// Service layer wrapper class for accessing the MapBox API
    /// </summary>
    public class MappingService : IMappingService
    {
        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/>
        /// </summary>
        private readonly IEnvironmentManager _environmentManager;

        /// <summary>
        /// Instance of <see cref="RestClient"/>
        /// </summary>
        private readonly RestClient _client;

        /// <summary>
        /// Zoom level to be sent to MapBox when retrieving static map images
        /// </summary>
        private const string StaticMapsZoomLevel = "12.5";

        /// <summary>
        /// Map resolution sent to MapBox when receiving static map images
        /// </summary>
        private const string StaticMapsResolution = "400x275@2x";

        /// <summary>
        /// Initialise a new instance of the <see cref="MappingService"/> class
        /// </summary>
        /// <param name="environmentManager">Instance of <see cref="IEnvironmentManager"/></param>
        /// <param name="httpClientService">Instance of <see cref="IHttpClientService"/></param>
        public MappingService(
            IEnvironmentManager environmentManager,
            IHttpClientService httpClientService)
        {
            this._environmentManager = environmentManager;
            this._client = httpClientService.NewClient();
        }

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
        public async Task<IList<FwdGeocodeResult>> ForwardGeocodeAsync(string query)
        {
            // Validate query against https://docs.mapbox.com/api/search/geocoding/#forward-geocoding
            query = query.Trim();

            if (query.Split(' ').Length > 20)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(query),
                    "Query needs to be no more than 20 words");
            }

            if (query.Contains(';'))
            {
                throw new ArgumentException("Query contains illegal character ';'", nameof(query));
            }

            RestRequest req = new(this.GenFwdGeocodeUrl(query));

            req.AddParameter("autocomplete", true);
            req.AddParameter("access_token", this._environmentManager.MapBoxApiKey);

            RestResponse res = await this._client.GetAsync(req);

            if (res.Content == null)
            {
                return new List<FwdGeocodeResult>();
            }

            JObject jObject = JObject.Parse(res.Content);
            JArray locationArray = JArray.Parse(jObject["features"]?.ToString() ?? "[]");

            return locationArray.Select(f => new FwdGeocodeResult
            {
                Text = (string)f["text"],
                PlaceName = (string)f["place_name"],
                Longitude = (float)f["geometry"]["coordinates"]?[0],
                Latitude = (float)f["geometry"]["coordinates"]?[1]
            }).ToList();
        }

        /// <summary>
        /// Get a static map image from the MapBox API
        /// </summary>
        /// <param name="longitude">The longitude of the map</param>
        /// <param name="latitude">The latitude of the map</param>
        /// <returns>A byte array representing the contents of the image</returns>
        public async Task<byte[]> GetStaticMapImageAsync(float longitude, float latitude)
        {
            RestRequest req = new(this.GenStaticMapImageUrl(longitude, latitude));
            req.AddParameter("access_token", this._environmentManager.MapBoxApiKey);
            return await this._client.DownloadDataAsync(req);
        }

        /// <summary>
        /// URL encode the query and return it appended to the base forward geocode URL
        /// </summary>
        /// <param name="validatedQuery">A validated query value</param>
        /// <returns>The base forward geocode URL complete with URL encoded query</returns>
        private string GenFwdGeocodeUrl(string validatedQuery)
        {
            validatedQuery = HttpUtility.UrlEncode(validatedQuery);
            return $"{this._environmentManager.BaseMapBoxForwardGeocodeUrl}{validatedQuery}.json";
        }

        /// <summary>
        /// Generate a static maps url for a given location that can be used to retrieve the
        /// image from the MapBox API
        /// </summary>
        /// <param name="longitude">The longitude of the map</param>
        /// <param name="latitude">The latitude of the map</param>
        /// <returns>A complete URL that can be contacted for a static map image</returns>
        private string GenStaticMapImageUrl(float longitude, float latitude)
        {
            // Add a pin to the center of the static map as an overlay.
            // As the image is centered on the coordinates, they can also be used for the pin position itself
            string pinOverlay = $"pin-s+999({longitude},{latitude})";
            return $"{this._environmentManager.BaseMapBoxStaticMapsUrl}{pinOverlay}/{longitude},{latitude},{StaticMapsZoomLevel},0/{StaticMapsResolution}";
        }
    }
}
