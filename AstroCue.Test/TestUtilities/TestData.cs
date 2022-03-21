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
        public const string MapBoxForwardGeocodeSample = "{\"type\":\"...\",\"query\":[\"test\"],\"features\":[{\"id\":\"1\",\"type\":\"Feature\",\"place_type\":[\"poi\"],\"relevance\":1,\"properties\":{\"foursquare\":\"test\",\"landmark\":true,\"address\":\"address 1\",\"category\":\"test\"},\"text\":\"Test 1\",\"place_name\":\"Test address 1\",\"center\":[29.030378,41.144889],\"geometry\":{\"coordinates\":[-0.102327,51.527388],\"type\":\"Point\"},\"context\":[{\"id\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"text\":\"...\"},{\"id\":\"...\",\"wikidata\":\"...\",\"short_code\":\"...\",\"text\":\"...\"}]}],\"attribution\":\"...\"}";
    }
}
