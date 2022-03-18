namespace AstroCue.Server.Services.Interfaces
{
    using RestSharp;

    /// <summary>
    /// Interface for <see cref="HttpClientService"/>
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Create a new <see cref="RestClient"/> with basic HTTP authentication
        /// </summary>
        /// <param name="apiKey">The API key</param>
        /// <param name="baseUrl">The base URL</param>
        /// <returns>A new <see cref="RestClient"/> instance</returns>
        RestClient NewClientBasicAuth(string apiKey, string baseUrl);
    }
}