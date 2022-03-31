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
        /// <summary>
        /// Given an <see cref="AstronomicalObject"/>, a <see cref="DateTime"/> and a
        /// pair of coordinates representing the observer's location, derive a pair of coordinates
        /// in the horizontal coordinate system, representing the position of <paramref name="obj"/> at
        /// instant <paramref name="instant"/>, with the observer's local horizon as the fundamental plane.
        ///
        /// Uses equations 13.5 & 13.6 from Meeus 1998 p. 91-93
        /// </summary>
        /// <param name="obj">An <see cref="AstronomicalObject"/> instance</param>
        /// <param name="instant">A <see cref="DateTime"/> instance</param>
        /// <param name="longitude">Observer's longitude</param>
        /// <param name="latitude">Observer's latitude</param>
        /// <returns>An instance of <see cref="AltAz"/></returns>
        /// <exception cref="ArgumentException">If <paramref name="instant"/> is not UTC</exception>
        public static AltAz EquatorialToHorizontal(AstronomicalObject obj, DateTime instant, float longitude, float latitude)
        {
            if (instant.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Date needed in UTC", nameof(instant));
            }

            // first, convert the object's Right Ascension and Declination values to degrees
            double raDeg = obj.RightAscension.Hours + obj.RightAscension.Minutes / 60 +
                           obj.RightAscension.Seconds / 3600;

            double decDeg = obj.Declination.Degrees + obj.RightAscension.Minutes / 60 +
                            obj.Declination.Seconds / 3600;
            
            // and then convert to radians
            double raRad = DegToRad(raDeg);
            double decRad = DegToRad(decDeg);

            // convert observation latitude to radians
            double latitudeRad = DegToRad(Convert.ToDouble(latitude));

            // calculate hour angle (Meeus, 1998, p. 92)
            double hourAngleDeg = MeanSiderealTimeAtInstant(instant) - longitude - raDeg;
            hourAngleDeg = BringInRange360(hourAngleDeg);

            double hourAngleRad = DegToRad(hourAngleDeg);

            // calculate local altitude and azimuth (Meeus 1998, p. 93) equations 13.5 & 13.6
            double azimuthRad = Math.Sin(hourAngleRad)
                                / (Math.Cos(hourAngleRad) * Math.Sin(latitudeRad) -
                                   Math.Tan(decRad) * Math.Cos(latitudeRad));

            double altitudeRad = Math.Sin(latitudeRad) * Math.Sin(decRad) +
                                 Math.Cos(latitudeRad) * Math.Cos(decRad) * Math.Cos(hourAngleRad);

            AltAz coordinates = new()
            {
                Azimuth = (float)RadToDeg(Math.Atan(azimuthRad)),
                Altitude = (float)RadToDeg(Math.Asin(altitudeRad))
            };

            return coordinates;
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
            mst = BringInRange360(mst);

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

        /// <summary>
        /// Brings a value in range using n number of multiples of 360 degrees
        /// </summary>
        /// <param name="val">A value</param>
        /// <returns><paramref name="val"/> in range 0-360 degrees</returns>
        private static double BringInRange360(double val)
        {
            if (val < 360)
            {
                while (val < 0)
                {
                    val += 360;
                }
            }

            if (val > 360)
            {
                while (val > 360)
                {
                    val -= 360;
                }
            }

            return val;
        }

        /// <summary>
        /// Converts a degrees value to radians
        /// </summary>
        /// <param name="deg">A value in degrees</param>
        /// <returns><paramref name="deg"/> in radians</returns>
        private static double DegToRad(double deg)
        {
            return deg * Math.PI / 180;
        }

        /// <summary>
        /// Converts a radians value to degrees
        /// </summary>
        /// <param name="rad">A value in radians</param>
        /// <returns><paramref name="rad"/> in degrees</returns>
        private static double RadToDeg(double rad)
        {
            return rad * 180 / Math.PI;
        }
    }
}
