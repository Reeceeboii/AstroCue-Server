namespace AstroCue.Server.Astronomy
{
    using Models.Misc;

    /// <summary>
    /// Class for rating weather forecasts for astronomical observations
    /// </summary>
    public static class WeatherForecastRating
    {
        /// <summary>
        /// Calculate an 'index' for the quality of a forecast for astronomical observations.
        /// Lower index = better conditions for observing
        /// </summary>
        /// <param name="hourlyForecast">An instance of <see cref="HourlyForecast"/></param>
        /// <returns>An index representing the observing conditions for <paramref name="hourlyForecast"/></returns>
        public static float CalculateIndex(HourlyForecast hourlyForecast)
        {
            float index = 0f;

            index += hourlyForecast.CloudCoveragePercent;
            index += hourlyForecast.WindSpeedMetersPerSec;
            index += hourlyForecast.ProbabilityOfPrecipitation;
            index += hourlyForecast.HumidityPercent;

            return index;
        }
    }
}
