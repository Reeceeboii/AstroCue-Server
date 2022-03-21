namespace AstroCue.Test.TestUtilities
{
    /// <summary>
    /// Class holding various bits of test data
    /// </summary>
    internal static class TestData
    {
        /// <summary>
        /// A fake example of a MapBox API forward geocode response
        /// </summary>
        public const string MapBoxForwardGeocodeSample = 
            "{\"type\":\"...\",\"query\":[\"test\"],\"features\":[{\"id\":\"1\",\"type\":\"Feature\",\"place_type\":[\"poi\"],\"relevance\":1,\"properties\":{\"foursquare\":\"test\",\"landmark\":true,\"address\":\"address 1\",\"category\":\"test\"},\"text\":\"Test 1\",\"place_name\":\"Test address 1\",\"center\":[29.030378,41.144889],\"geometry\":{\"coordinates\":[-0.102327,51.527388],\"type\":\"Point\"},\"context\":[{\"id\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"short_code\":\"...\",\"text\":\"...\"}]}],\"attribution\":\"...\"}";

        /// <summary>
        /// A fake example of an OpenWeatherMap current weather response
        /// </summary>
        public const string OwmCurrentWeatherSample =
            "{\"coord\":{\"lon\":-0.1023,\"lat\":51.5274},\"weather\":[{\"id\":804,\"main\":\"Clouds\",\"description\":\"overcast clouds\",\"icon\":\"04d\"}],\"base\":\"stations\",\"main\":{\"temp\":289.08,\"feels_like\":287.9,\"temp_min\":287.49,\"temp_max\":290.23,\"pressure\":1027,\"humidity\":45},\"visibility\":10000,\"wind\":{\"speed\":4.12,\"deg\":100},\"clouds\":{\"all\":92},\"dt\":1647880043,\"sys\":{\"type\":2,\"id\":2006068,\"country\":\"GB\",\"sunrise\":1647842471,\"sunset\":1647886442},\"timezone\":0,\"id\":6690574,\"name\":\"Clerkenwell\",\"cod\":200}";
    }
}
