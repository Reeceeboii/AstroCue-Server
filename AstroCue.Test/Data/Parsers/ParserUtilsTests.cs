namespace AstroCue.Test.Data.Parsers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Data.Parsers;
    using Server.Entities.Owned;

    /// <summary>
    /// Tests targeting <see cref="ParserUtils"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ParserUtilsTests
    {
        /// <summary>
        /// Tests that a valid right ascension value can be parsed
        /// </summary>
        [TestMethod]
        public void ParseRightAscensionTest()
        {
            // Arrange
            const string ra = "12 44 06.9";

            // Act
            RightAscension raParsed = ParserUtils.ParseRightAscension(ra);

            // Assert
            raParsed.Should().NotBeNull();
            raParsed.Hours.Should().Be(12);
            raParsed.Minutes.Should().Be(44);
            raParsed.Seconds.Should().Be(6.9);
        }

        /// <summary>
        /// Tests that an invalid right ascension value cannot be parsed
        /// </summary>
        [TestMethod]
        public void ParseRightAscensionErrorTest()
        {
            // Arrange
            const string ra = "12 06.9";

            // Act
            // Assert
            Assert.ThrowsException<FormatException>(() => ParserUtils.ParseRightAscension(ra));
        }

        /// <summary>
        /// Tests that a valid positive right declination value can be parsed
        /// </summary>
        [TestMethod]
        public void ParseDeclinationPositiveTest()
        {
            // Arrange
            const string dec = "+25 55 21";

            // Act
            Declination decParsed = ParserUtils.ParseDeclination(dec);

            // Assert
            decParsed.Should().NotBeNull();
            decParsed.Degrees.Should().Be(25);
            decParsed.Minutes.Should().Be(55);
            decParsed.Seconds.Should().Be(21);
        }

        /// <summary>
        /// Tests that a valid positive right declination value can be parsed
        /// </summary>
        [TestMethod]
        public void ParseDeclinationNegativeTest()
        {
            // Arrange
            const string dec = "-25 55 21";

            // Act
            Declination decParsed = ParserUtils.ParseDeclination(dec);

            // Assert
            decParsed.Should().NotBeNull();
            decParsed.Degrees.Should().Be(-25);
            decParsed.Minutes.Should().Be(55);
            decParsed.Seconds.Should().Be(21);
        }

        /// <summary>
        /// Tests that an invalid declination value cannot be parsed
        /// </summary>
        [TestMethod]
        public void ParseDeclinationErrorTest()
        {
            // Arrange
            const string dec = "12 06.9";

            // Act
            // Assert
            Assert.ThrowsException<FormatException>(() => ParserUtils.ParseDeclination(dec));
        }
    }
}
