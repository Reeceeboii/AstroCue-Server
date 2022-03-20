namespace AstroCue.Server.Astronomy
{
    using System;

    /// <summary>
    /// Class to handle Bortle Scale operations
    /// https://en.wikipedia.org/wiki/Bortle_scale
    /// </summary>
    public static class BortleScale
    {
        /// <summary>
        /// Converts a milicandela/m^2 value to a Bortle Scale value. Uses the International Year of Astronomy's
        /// Sky Brightness Nomogram (http://www.darkskiesawareness.org/img/sky-brightness-nomogram.gif) for an approximate
        /// conversion
        /// </summary>
        /// <param name="mcdM2">A milicandella/m^2 value</param>
        /// <returns>The value's approximate Bortle Scale value</returns>
        public static int McdM2ToBortle(float mcdM2)
        {
            return mcdM2 switch
            {
                <= 0.25f => 1,
                > 0.25f and <= 0.275f => 2,
                > 0.275f and <= 0.325f => 3,
                > 0.325f and <= 0.5f => 4,
                > 0.5f and <= 2.5f => 5,
                > 2.5f and <= 4f => 6,
                > 4f and <= 6.85f => 7,
                _ => 8
            };
        }

        /// <summary>
        /// Converts a Bortle Scale value to its string name.
        /// https://skyandtelescope.org/astronomy-resources/light-pollution-and-astronomy-the-bortle-dark-sky-scale/
        /// </summary>
        /// <param name="bortle">A Bortle Scale value</param>
        /// <returns>A string representation of the value</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ScaleToDescription(int bortle)
        {
            return bortle switch
            {
                1 => "Excellent Dark-sky Site",
                2 => "Typical Truly Dark Site",
                3 => "Rural Sky",
                4 => "Rural/Suburban Transition",
                5 => "Suburban Sky",
                6 => "Bright Suburban Sky",
                7 => "Suburban/Urban Transition",
                8 => "City or inner city sky",
                _ => throw new ArgumentOutOfRangeException(nameof(bortle), "Argument out of range")
            };
        }
    }
}
