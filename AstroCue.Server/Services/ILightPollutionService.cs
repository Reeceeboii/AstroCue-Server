namespace AstroCue.Server.Services
{
    using System;
    using Models.Misc;

    /// <summary>
    /// Interface for <see cref="LightPollutionService"/>
    /// </summary>
    public interface ILightPollutionService
    {
        /// <summary>
        /// Retrieve light pollution data for a given set of coordinates
        /// </summary>
        /// <param name="longitude">The longitude value to query</param>
        /// <param name="latitude">The latitude value to query</param>
        /// <returns>An instance of <see cref="LightPollution"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">If the coordinates lie outside of the dataset</exception>
        LightPollution GetLightPollutionForCoords(float longitude, float latitude);
    }
}