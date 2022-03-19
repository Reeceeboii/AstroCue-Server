namespace AstroCue.Server.Data.Parsers
{
    using System;
    using Entities.Owned;

    /// <summary>
    /// Utility functions that can be shared between different parser classes
    /// </summary>
    public static class ParserUtils
    {
        /// <summary>
        /// Converts a string representation of a right ascension value (H|M|S) to a <see cref="RightAscension"/> instance.
        /// Example input is "07 10 40.80"
        /// </summary>
        /// <param name="ra">String representation of the right ascension value, read directly from the catalogue</param>
        /// <returns><param name="ra"> represented as a <see cref="RightAscension"/></param></returns>
        public static RightAscension ParseRightAscension(string ra)
        {
            // split Ra value into 3 elements using the catalogue's spaces as a delimeter
            string[] elements = ra.Split(' ');

            // parse
            int hours = Convert.ToInt32(elements[0]);
            int minutes = Convert.ToInt32(elements[1]);
            double seconds = Convert.ToDouble(elements[2]);

            return new RightAscension()
            {
                Hours = hours,
                Minutes = minutes,
                Seconds = seconds
            };
        }

        /// <summary>
        /// Converts a string representation of a declination value (Deg|M|S) to a <see cref="Declination"/> instance.
        /// Example input is "+42 08 29.4"
        /// </summary>
        /// <param name="dec">String representation of the declination value, read directly from the catalogue</param>
        /// <returns><paramref name="dec"/> represented as a <see cref="Declination"/></returns>
        public static Declination ParseDeclination(string dec)
        {
            // split Dec value into 3 elements using the catalogue's spaces as a delimeter
            string[] elements = dec.Split(' ');

            // parse
            int degrees = Convert.ToInt32(elements[0]);
            int minutes = Convert.ToInt32(elements[1]);
            double seconds = Convert.ToDouble(elements[2]);

            return new Declination()
            {
                Degrees = degrees,
                Minutes = minutes,
                Seconds = seconds
            };
        }
    }
}
