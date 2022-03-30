namespace AstroCue.Server.Astronomy
{
    using System;
    using Entities;
    using Entities.Owned;

    /// <summary>
    /// Class to carry out various coordinate transformation operations
    /// </summary>
    public static class CoordinateTransformations
    {
        public static AltAz EquatorialToHorizontal(AstronomicalObject obj, DateTime date, float longitude, float latitude)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Date needed in UTC", nameof(date));
            }


        }

        /// <summary>
        /// Calculates the mean sidereal time at Greenwich for any instant UTC
        /// https://en.wikipedia.org/wiki/Sidereal_time
        /// </summary>
        /// <param name="instant">A <see cref="DateTime"/> instance in UTC</param>
        /// <returns>Mean sidereal time at Greenwich for the instant given</returns>
        public static double MeanSiderealTimeAtInstant(DateTime instant)
        {
            if (instant.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Date needed in UTC", nameof(instant));
            }

            double jd = DateToJulianDay(instant);
            double t = (jd - 2451545.0) / 36525;
            
            double mst = 280.46061837 + 360.98564736629 * (jd - 2451545.0) + 0.000387933 * (t * t) - t * t * t / 3871000;

            // bring in range 0-360
            if (mst < 360)
            {
                while (mst < 0)
                {
                    mst += 360;
                }
            }

            if (mst > 360)
            {
                while (mst > 360)
                {
                    mst -= 360;
                }
            }

            return mst;
        }

        /// <summary>
        /// Calculate the Julian Day of a given date
        /// https://en.wikipedia.org/wiki/Julian_day
        /// 
        /// "The Julian Day number or, more simply, the Julian Day (*) (JD) is a continuous
        /// count of days and fractions thereof from the beginning of the year -4712. By tradition,
        /// the Julian Day begins at Greenwich mean noon, that is, at 12h Universal Time."
        /// (Meeus, 1998, p. 59-66)
        /// </summary>
        /// <param name="instant">A <see cref="DateTime"/> instance in UTC</param>
        /// <returns>The date's Julian Day</returns>
        public static double DateToJulianDay(DateTime instant)
        {
            if (instant.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Date needed in UTC", nameof(instant));
            }

            int year = instant.Year;
            int month = instant.Month;
            double dayFrac = instant.Hour / 24.0 + instant.Minute / 1440.0 + instant.Day;

            if (!(month > 2))
            {
                year -= 1;
                month += 12;
            }

            int a = (int)Math.Floor(year / 100.0);
            int b = 2 - a + (int)Math.Floor(a / 4.0);
             
            return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + dayFrac + b - 1524.5;
        } 
    }
}
