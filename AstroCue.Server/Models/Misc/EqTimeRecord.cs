namespace AstroCue.Server.Models.Misc
{
    /// <summary>
    /// An easily comparable record representing some info from a DateTime type
    /// </summary>
    public record EqTimeRecord
    {
        /// <summary>
        /// Gets and sets the day of the month
        /// </summary>
        public int Day { get; init; }

        /// <summary>
        /// Gets and sets the hour of the day
        /// </summary>
        public int Hour { get; init; }
    }
}
