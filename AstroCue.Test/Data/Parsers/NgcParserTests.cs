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
    /// Test class targeting <see cref="NgcParser"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class NgcParserTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private NgcParser _sut;

        /// <summary>
        /// Mock <see cref="IFileSystem"/> instance used by the tests
        /// </summary>
        private readonly Mock<IFileSystem> _mockFileSystem;

        /// <summary>
        /// <see cref="RightAscension"/> and <see cref="Declination"/> values to be used by the tests
        /// </summary>
        private const int ExpectedCatalogueIndentifier = 3697;
        private const int ExpectedRightAscensionHours = 11;
        private const int ExpectedRightAscensionMinutes = 28;
        private const double ExpectedRightAscensionSeconds = 42.7;
        private const int ExpectedDeclinationDegrees = 20;
        private const int ExpectedDeclinationMinutes = 47;
        private const double ExpectedDeclinationSeconds = 44;
        private const float ExpectedApparentMagnitude = 14.0f;

        /// <summary>
        /// A correctly formatted few lines from the NGC for use in tests
        /// </summary>
        private readonly string[] _correctlyFormattedCatalogue =
        {
            "11 28 42.7|+20 47 44|3697|A| 5|14.0",
            "11 28 54.7|+20 42 44|3697|B| 5|15.5",
            "11 28 54.7|+20 43 44|3698| | 5|15.5"
        };

        /// <summary>
        /// Sample section of the NGC name catalogue
        /// </summary>
        private readonly string[] _nameCatalogue =
        {
            "Test1                              | 3697",
            "Test2                              | 3698"
        };

        /// <summary>
        /// A few lines from the NGC with a right ascension and declination missing across 2 different entries
        /// </summary>
        private readonly string[] _catalogueMissingRaAndDec =
        {
            "11 28 42.7|+20 47 44|3697|A| 5|14.0",
            "          |+20 42 44|3697|B| 5|15.5",
            "11 28 54.7|         |3698| | 5|15.5"
        };

        /// <summary>
        /// A few lines from the NGC with one of the entries missing an apparent magnitude value 
        /// </summary>
        private readonly string[] _catalogueMissingApparentMagnitude =
        {
            "11 28 42.7|+20 47 44|3697|A| 5|14.0",
            "11 28 54.7|+20 42 44|3697|B| 5|    ",
            "11 28 54.7|+20 43 44|3698| | 5|15.5"
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="NgcParserTests"/> class
        /// </summary>
        public NgcParserTests()
        {
            this._mockFileSystem = new Mock<IFileSystem>();
        }

        /// <summary>
        /// Tests that an instance of <see cref="NgcParser"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseNgcParserTest()
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

            result[0].Name.Should().Be("Test1");
            result[0].OfficiallyNamed.Should().BeTrue();

            result[1].Name.Should().Be("Test1");
            result[1].OfficiallyNamed.Should().BeTrue();

            result[2].Name.Should().Be("Test2");
            result[2].OfficiallyNamed.Should().BeTrue();

            ((NgcObject)result[0]).Type.Should().Be("Galaxy");
            ((NgcObject)result[0]).PartOfMultipleSystem.Should().Be(true);

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
                .Setup(i => i.Path.Join(It.IsAny<string>(), It.Is<string>(s => s == "VII_1B_catalog.tsv")))
                .Returns("datacataloguepath");

            this._mockFileSystem
                .Setup(i => i.Path.Join(It.IsAny<string>(), It.Is<string>(s => s == "VII_1B_catalog_names.tsv")))
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
        /// Initialises a new intance of the <see cref="NgcParser"/> class for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new NgcParser(this._mockFileSystem.Object);
        }
    }
}
