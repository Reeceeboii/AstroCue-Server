namespace AstroCue.Test.Data.Parsers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Abstractions;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Data.Parsers;
    using Server.Entities;
    using Server.Entities.Owned;

    /// <summary>
    /// Tests targeting the <see cref="HipParser"/> class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HipParserTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private HipParser _sut;

        /// <summary>
        /// Mock <see cref="IFileSystem"/> instance used by the tests
        /// </summary>
        private readonly Mock<IFileSystem> _mockFileSystem;

        /// <summary>
        /// <see cref="HipObject"/> and <see cref="RightAscension"/>/<see cref="Declination"/> expected
        /// values to be used by the tests
        /// </summary>
        private const int ExpectedCatalogueIndentifier = 59501;
        private const int ExpectedRightAscensionHours = 12;
        private const int ExpectedRightAscensionMinutes = 12;
        private const double ExpectedRightAscensionSeconds = 09.2903464;
        private const int ExpectedDeclinationDegrees = 20;
        private const int ExpectedDeclinationMinutes = 32;
        private const double ExpectedDeclinationSeconds = 31.429778;
        private const float ExpectedApparentMagnitude = 5.60f;

        /// <summary>
        /// A correctly formatted few lines from the Hipparcos catalogue for use in tests
        /// </summary>
        private readonly string[] _correctlyFormattedCatalogue =
        {
            "12 12 09.2903464|+20 32 31.429778| 59501| 5.60",
            "12 12 10.2760619|-63 27 14.813107| 59502| 6.98",
            "12 12 10.8985473|-58 49 05.988300| 59503| 8.53"
        };

        /// <summary>
        /// Sample section of the Hippcaros name catalogue from the IAU WGSN
        /// </summary>
        private readonly string[] _nameCatalogue =
        {
            "test1             Dubhe             HR 4301      alf   α     UMa A    11037+6145  1.81  59502  95689 165.931965  61.751035 2016-06-30",
            "test2             Dubhe             HR 4301      alf   α     UMa A    11037+6145  1.81  59503  95689 165.931965  61.751035 2016-06-30",
        };

        /// <summary>
        /// A few lines from the Hipparcos catalogue with a right ascension and declination missing across 2 different entries
        /// </summary>
        private readonly string[] _catalogueMissingRaAndDec =
        {
            "12 12 09.2903464|+20 32 31.429778| 59501| 5.60",
            "                |-63 27 14.813107| 59502| 6.98",
            "12 12 10.8985473|                | 59503| 8.53"
        };

        /// <summary>
        /// A few lines from the Hipparcos catalogue with one of the entries missing an apparent magnitude value 
        /// </summary>
        private readonly string[] _catalogueMissingApparentMagnitude =
        {
            "12 12 09.2903464|+20 32 31.429778| 59501| 5.60",
            "12 12 10.2760619|-63 27 14.813107| 59502|     ",
            "12 12 10.8985473|-58 49 05.988300| 59503| 8.53"
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="HipParserTests"/> class
        /// </summary>
        public HipParserTests()
        {
            this._mockFileSystem = new Mock<IFileSystem>();
        }

        /// <summary>
        /// Tests that an instance of <see cref="HipParser"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialisesHipParserTest()
        {
            // Arrange
            this.PrepMocksToReadCatalogues(this._correctlyFormattedCatalogue, this._nameCatalogue);

            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
            this._mockFileSystem
                .Verify(m => m.Path.Join(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            this._mockFileSystem
                .Verify(m => m.File.Exists(It.IsAny<string>()), Times.Exactly(2));
        }

        /// <summary>
        /// Test that the parser can correctly initialise and parse a correctly formatted catalogue
        /// </summary>
        [TestMethod]
        public void CanParseCatalogueTest()
        {
            // Arrange
            this.PrepMocksToReadCatalogues(this._correctlyFormattedCatalogue, this._nameCatalogue);

            // Act
            this.CreateSut();
            List<AstronomicalObject> result = this._sut.ParseCatalogue();

            // Assert
            this._mockFileSystem
                .Verify(m => m.File.Exists(It.IsAny<string>()), Times.Exactly(2));
            this._mockFileSystem
                .Verify(m => m.File.ReadAllLines(It.IsAny<string>()), Times.Exactly(2));

            result.Count.Should().Be(this._correctlyFormattedCatalogue.Length);

            result[0].Name.Should().Be(null);
            result[1].Name.Should().Be("test1");
            result[2].Name.Should().Be("test2");

            result[0].RightAscension.Should().NotBeNull();
            result[0].Declination.Should().NotBeNull();
            result[0].CatalogueIdentifier.Should().Be(ExpectedCatalogueIndentifier);

            result[0].RightAscension.Hours.Should().Be(ExpectedRightAscensionHours);
            result[0].RightAscension.Minutes.Should().Be(ExpectedRightAscensionMinutes);
            result[0].RightAscension.Seconds.Should().Be(ExpectedRightAscensionSeconds);

            result[0].Declination.Degrees.Should().Be(ExpectedDeclinationDegrees);
            result[0].Declination.Minutes.Should().Be(ExpectedDeclinationMinutes);
            result[0].Declination.Seconds.Should().Be(ExpectedDeclinationSeconds);

            result[0].ApparentMagnitude.Should().Be(ExpectedApparentMagnitude);
        }

        /// <summary>
        /// Tests that a missing catalogue file results in a <see cref="FileNotFoundException"/>
        /// </summary>
        [TestMethod]
        public void ParseCatalogueFileMissingTest()
        {
            // Arrange
            this._mockFileSystem
                .Setup(i => i.Path.Join(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("...");

            this._mockFileSystem
                .Setup(i => i.File.Exists(It.IsAny<string>()))
                .Returns(false);

            // Act
            // Assert
            Assert.ThrowsException<FileNotFoundException>(this.CreateSut);

            // error should have been thrown at the first missing catalogue
            this._mockFileSystem
                .Verify(m => m.File.Exists(It.IsAny<string>()), Times.Once());
        }

        /// <summary>
        /// Test that the parser can correctly initialise and parse a catalogue missing an apparent mag value
        /// </summary>
        [TestMethod]
        public void CanParseCatalogueMissingApparentMagnitudeTest()
        {
            // Arrange
            this.PrepMocksToReadCatalogues(this._catalogueMissingApparentMagnitude, this._nameCatalogue);

            // Act
            this.CreateSut();
            List<AstronomicalObject> result = this._sut.ParseCatalogue();

            // Assert
            this._mockFileSystem
                .Verify(m => m.File.Exists(It.IsAny<string>()), Times.Exactly(2));
            this._mockFileSystem
                .Verify(m => m.File.ReadAllLines(It.IsAny<string>()), Times.Exactly(2));

            result.Count.Should().Be(2);
        }

        /// <summary>
        /// Test that the parser can correctly initialise and parse a catalogue missing RA and DEC values
        /// </summary>
        [TestMethod]
        public void CanParseCatalogueMissingCoordinatesTest()
        {
            // Arrange
            this.PrepMocksToReadCatalogues(this._catalogueMissingRaAndDec, this._nameCatalogue);

            // Act
            this.CreateSut();
            List<AstronomicalObject> result = this._sut.ParseCatalogue();

            // Assert
            this._mockFileSystem
                .Verify(m => m.File.Exists(It.IsAny<string>()), Times.Exactly(2));
            this._mockFileSystem
                .Verify(m => m.File.ReadAllLines(It.IsAny<string>()), Times.Exactly(2));

            result.Count.Should().Be(1);
        }

        /// <summary>
        /// Preps the mocks to read the catalogues correctly
        /// </summary>
        /// <param name="dataCatalogue">The data catalogue content</param>
        /// <param name="nameCatalogue">The name catalogue content</param>
        private void PrepMocksToReadCatalogues(string[] dataCatalogue, string[] nameCatalogue)
        {
            this._mockFileSystem
                .Setup(i => i.Path.Join(It.IsAny<string>(), It.Is<string>(s => s == "I239_hip_main.tsv")))
                .Returns("datacataloguepath");

            this._mockFileSystem
                .Setup(i => i.Path.Join(It.IsAny<string>(), It.Is<string>(s => s == "I239_hip_main_names.tsv")))
                .Returns("namecataloguepath");

            this._mockFileSystem
                .Setup(i => i.File.Exists(It.IsAny<string>()))
                .Returns(true);

            this._mockFileSystem
                .Setup(i => i.File.ReadAllLines(It.Is<string>(s => s == "datacataloguepath")))
                .Returns(dataCatalogue);

            this._mockFileSystem
                .Setup(i => i.File.ReadAllLines(It.Is<string>(s => s == "namecataloguepath")))
                .Returns(nameCatalogue);
        }

        /// <summary>
        /// Initialises a new intance of the <see cref="HipParser"/> class for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new HipParser(this._mockFileSystem.Object);
        }
    }
}
