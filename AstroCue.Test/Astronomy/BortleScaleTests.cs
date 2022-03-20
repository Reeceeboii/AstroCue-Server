namespace AstroCue.Test.Astronomy
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Astronomy;

    /// <summary>
    /// Tests targeting <see cref="BortleScale"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BortleScaleTests
    {
        /// <summary>
        /// Tests the <see cref="BortleScale.McdM2ToBortle"/> method
        /// </summary>
        [TestMethod]
        public void McdM2ToBortleTest()
        {
            // Arrange
            // Act
            // Assert
            BortleScale.McdM2ToBortle(0.15f).Should().Be(1);
            BortleScale.McdM2ToBortle(0.26f).Should().Be(2);
            BortleScale.McdM2ToBortle(0.3f).Should().Be(3);
            BortleScale.McdM2ToBortle(0.42f).Should().Be(4);
            BortleScale.McdM2ToBortle(1.2f).Should().Be(5);
            BortleScale.McdM2ToBortle(2.6f).Should().Be(6);
            BortleScale.McdM2ToBortle(5.5f).Should().Be(7);
            BortleScale.McdM2ToBortle(8f).Should().Be(8);
            BortleScale.McdM2ToBortle(100f).Should().Be(8);
        }

        /// <summary>
        /// Tests the <see cref="BortleScale.ScaleToDescription"/> method
        /// </summary>
        [TestMethod]
        public void ScaleToDescriptionTest()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                BortleScale.ScaleToDescription(0));
            BortleScale.ScaleToDescription(1).Should().Be("Excellent Dark-sky Site");
            BortleScale.ScaleToDescription(2).Should().Be("Typical Truly Dark Site");
            BortleScale.ScaleToDescription(3).Should().Be("Rural Sky");
            BortleScale.ScaleToDescription(4).Should().Be("Rural/Suburban Transition");
            BortleScale.ScaleToDescription(5).Should().Be("Suburban Sky");
            BortleScale.ScaleToDescription(6).Should().Be("Bright Suburban Sky");
            BortleScale.ScaleToDescription(7).Should().Be("Suburban/Urban Transition");
            BortleScale.ScaleToDescription(8).Should().Be("City or inner city sky");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                BortleScale.ScaleToDescription(9));
        }
    }
}
