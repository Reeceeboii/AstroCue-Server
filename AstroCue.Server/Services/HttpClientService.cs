namespace AstroCue.Server.Services
{
    using System;
    using Interfaces;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Service to handle HTTP client options and <see cref="RestClient"/> injection
    /// </summary>
    public class HttpClientService : IHttpClientService
    {
        /// <summary>
        /// Create a new <see cref="RestClient"/> with basic HTTP authentication
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="baseUrl">The base URL</param>
        /// <returns>A new <see cref="RestClient"/> instance</returns>
        public RestClient NewClientBasicAuth(string apiKey, string baseUrl)
        {
            RestClientOptions opts = new()
            {
                BaseUrl = new Uri(baseUrl)
            };

            return new RestClient(opts)
            {
                Authenticator = new HttpBasicAuthenticator("api", apiKey)
            };
        }
    }
}
