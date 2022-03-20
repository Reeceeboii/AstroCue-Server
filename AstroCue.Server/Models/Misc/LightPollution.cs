namespace AstroCue.Server.Models.Misc
{
    /// <summary>
    /// Represents light pollution data for a given location
    /// </summary>
    public class LightPollution
    {
        /// <summary>
        /// The Bortle Scale value
        /// </summary>
        public int BortleValue { get; set; }

        /// <summary>
        /// The description applied to the <see cref="BortleValue"/>
        /// </summary>
        public string BortleDesc { get; set; }

        /// <summary>
        /// The raw value read from the GeoTIFF band, in mcd/m^2
        /// </summary>
        public float RawMilicandella { get; set; }
    }
}
