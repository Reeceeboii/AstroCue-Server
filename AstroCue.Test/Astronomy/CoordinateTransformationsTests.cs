namespace AstroCue.Test.Astronomy
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Server.Astronomy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Entities;
    using Server.Entities.Owned;

    /// <summary>
    /// Tests targeting <see cref="CoordinateTransformations"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CoordinateTransformationsTests
    {
        /// <summary>
        /// Tests the <see cref="CoordinateTransformations.EquatorialToHorizontal"/> method
        /// </summary>
        [TestMethod]
        public void EquatorialToHorizontalTest()
        {
            // Arrange
            const float cuolLongitude = -0.102338f;
            const float cuolLatitude = 51.527363f;

            // Test case (Meeus, 1998)
            // Venus on April 10th 1987 at 19:21:00 UTC from US Naval Observatory in Washington D.C.
            AstronomicalObject venus = new()
            {
                RightAscension = new RightAscension()
                {
                    Hours = 23,
                    Minutes = 9,
                    Seconds = 16.641
                },
                Declination = new Declination()
                {
                    Degrees = -6,
                    Minutes = 43,
                    Seconds = 11.61
                }
            };

            DateTime instant = DateTime.Parse("10 April 1987, 7:21 pm").AsUtc();
            const float longitude = 77.06556f;
            const float latitude = 38.92139f;

            const float venusExpectedAltitude = 15.1249f;
            const float venusExpectedAzimuth = 68.0337f + 180; // North v South Azimuthal reckoning

            // author's own test cases, expected values given from Stellarium software
            // ----------

            // Betelgeuse on 20th November 2026 at 04:09:00 UTC from City, University of London
            AstronomicalObject betelgeuse = new()
            {
                RightAscension = new RightAscension()
                {
                    Hours = 5,
                    Minutes = 55,
                    Seconds = 9.18
                },
                Declination = new Declination()
                {
                    Degrees = 7,
                    Minutes = 24,
                    Seconds = 22.8
                }
            };

            DateTime instant2 = DateTime.Parse("20 November 2026, 4:09 am").AsUtc();

            const float betelgeuseExpectedAltitude = 38.72908f;
            const float betelgeuseExpectedAzimuth = 222.3626f;

            // Act
            AltAz venusPosition = CoordinateTransformations.EquatorialToHorizontal(venus, instant, longitude, latitude);
            AltAz betelgeusePositition = CoordinateTransformations.EquatorialToHorizontal(betelgeuse, instant2, cuolLongitude, cuolLatitude);

            // Assert
            venusPosition.Altitude.Should().BeInRange(venusExpectedAltitude - 10, venusExpectedAltitude + 10);
            venusPosition.Azimuth.Should().BeInRange(venusExpectedAzimuth - 10, venusExpectedAzimuth + 10);

            betelgeusePositition.Altitude.Should()
                .BeInRange(betelgeuseExpectedAltitude - 10, betelgeuseExpectedAltitude + 10);
            betelgeusePositition.Azimuth.Should()
                .BeInRange(betelgeuseExpectedAzimuth - 10, betelgeuseExpectedAzimuth + 10);
        }

        /// <summary>
        /// Tests the <see cref="CoordinateTransformations.DateToJulianDay"/> method.
        /// Tests cases (Meeus, 1998) 
        /// </summary>
        [TestMethod]
        public void DateToJulianDayTest()
        {
            // Arrange
            DateTime test1 = DateTime.Parse("4 October 1957, 7:28 pm").AsUtc();
            DateTime test2 = DateTime.Parse("10 April 1987, 00:00 am").AsUtc();

            // Act
            double jd1 = CoordinateTransformations.DateToJulianDay(test1);
            double jd2 = CoordinateTransformations.DateToJulianDay(test2);

            // Assert
            Math.Round(jd1, 2).Should().Be(2436116.31);
            Math.Round(jd2, 1).Should().Be(2446895.5);
            Assert.ThrowsException<ArgumentException>(() =>
                CoordinateTransformations.DateToJulianDay(DateTime.Now.AsLocal()));
        }

        /// <summary>
        /// Tests the <see cref="CoordinateTransformations.MeanSiderealTimeAtInstant"/> method
        /// </summary>
        [TestMethod]
        public void MeanSiderealTimeAtInstantTest()
        {
            // Arrange
            DateTime test1 = DateTime.Parse("10 April 1987, 7:21 pm").AsUtc();

            // Act
            double mst1 = CoordinateTransformations.MeanSiderealTimeAtInstant(test1);

            // Assert
            Math.Round(mst1, 7).Should().Be(128.7378732);
            Assert.ThrowsException<ArgumentException>(() =>
                CoordinateTransformations.MeanSiderealTimeAtInstant(DateTime.Now.AsLocal()));
        }
    }
}
