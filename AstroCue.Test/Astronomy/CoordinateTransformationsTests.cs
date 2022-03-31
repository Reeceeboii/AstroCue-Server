namespace AstroCue.Test.Astronomy
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Server.Astronomy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests targeting <see cref="CoordinateTransformations"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CoordinateTransformationsTests
    {
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
