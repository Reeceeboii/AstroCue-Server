﻿namespace AstroCue.Test.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Abstractions;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Astronomy;
    using Server.Models.Misc;
    using Server.Services;
    using Server.Services.Interfaces;

    /// <summary>
    /// Tests targeting <see cref="LightPollutionService"/>
    /// </summary>
    ///
    /// NOTE:
    ///
    /// While I normally try to avoid any direct filesystem interaction during unit tests,
    /// the nature of GDAL and its operations simply cannot be tested without first reading
    /// a dataset from disk. Filesystem mocking and injection is used where possible, but GDAL
    /// itself still reads from the disk the same GeoTIFF dataset used during runtime.
    ///
    /// This disk reading is inherent to the GDAL binaries themselves and cannot be overriden
    /// (or at least I don't have sufficient knowledge to do so).
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LightPollutionServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private ILightPollutionService _sut;

        /// <summary>
        /// Mock <see cref="IFileSystem"/> used by the tests
        /// </summary>
        private readonly Mock<IFileSystem> _mockFileSystem;

        /// <summary>
        /// Initialises a new instance of the <see cref="LightPollutionServiceTests"/> class
        /// </summary>
        public LightPollutionServiceTests()
        {
            this._mockFileSystem = new Mock<IFileSystem>();
        }

        /// <summary>
        /// Tests that the <see cref="LightPollutionService"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseLightPollutionServiceTest()
        {
            // Arrange
            this.SetupMockFilesystemToReadDataset();

            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a <see cref="FileNotFoundException"/> is thrown if the dataset file is missing
        /// </summary>
        [TestMethod]
        public void DataSetFileNotFoundTest()
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
        }

        /// <summary>
        /// Attempts to read light pollution for a sample set of coordinates.
        /// Correct and expected values retrieved from dataset via QGIS:
        /// https://www.qgis.org/en/site/.
        ///
        /// Validates raw mcd/m^2 values and Bortle Scale values w/ their descriptions
        /// </summary>
        [TestMethod]
        public void ReadLightPollutionForLocation()
        {
            // Arrange
            this.SetupMockFilesystemToReadDataset();

            const float testLongitude1 = -0.0774f;
            const float testLatitude1 = 51.5265f;
            const float expectedMillicandella1 = 9.6913f;
            const int testExpectedBortle1 = 8;

            const float testLongitude2 = -2.744f;
            const float testLatitude2 = 56.867f;
            const float expectedMillicandella2 = 0.0280839f;
            const int testExpectedBortle2 = 1;

            // Act
            this.CreateSut();
            LightPollution test1 = this._sut.GetLightPollutionForCoords(testLongitude1, testLatitude1);
            LightPollution test2 = this._sut.GetLightPollutionForCoords(testLongitude2, testLatitude2);

            // Assert
            test1.Should().NotBeNull();
            test2.Should().NotBeNull();

            test1.BortleValue.Should().Be(testExpectedBortle1);
            test1.BortleDesc.Should().Be(BortleScale.ScaleToDescription(testExpectedBortle1));
            Math.Round(test1.RawMilicandella, 3).Should().Be(Math.Round(expectedMillicandella1, 3));

            test2.BortleValue.Should().Be(testExpectedBortle2);
            test2.BortleDesc.Should().Be(BortleScale.ScaleToDescription(testExpectedBortle2));
            Math.Round(test2.RawMilicandella, 3).Should().Be(Math.Round(expectedMillicandella2, 3));
        }

        /// <summary>
        /// Tests that reading data that lies outside of the dataset throws an error.
        ///
        /// CRS	EPSG:4326 - WGS 84 - Geographic
        /// Extent	-180.0000000000000000,-59.9957751150000007 : 179.9998560000000225,85.0541668649999991
        /// </summary>
        [TestMethod]
        public void ReadLightPollutionOutsideOfDatasetTest()
        {
            // Arrange
            this.SetupMockFilesystemToReadDataset();

            // +ve latitude out of range
            const float testLongitude1 = 0f;
            const float testLatitude1 = 86f;
            // -ve latitude out of range
            const float testLongitude2 = 0f;
            const float testLatitude2 = -61f;

            // +ve longitude out of range
            const float testLongitude3 = 180.5f;
            const float testLatitude3 = 0f;
            // -ve longitude out of range
            const float testLongitude4 = -180.5f;
            const float testLatitude4 = 0f;

            // Act
            this.CreateSut();

            // Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                this._sut.GetLightPollutionForCoords(testLongitude1, testLatitude1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                this._sut.GetLightPollutionForCoords(testLongitude2, testLatitude2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                this._sut.GetLightPollutionForCoords(testLongitude3, testLatitude3));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                this._sut.GetLightPollutionForCoords(testLongitude4, testLatitude4));
        }

        /// <summary>
        /// Setup the mock <see cref="IFileSystem"/> such that the dataset can be read successfully
        /// </summary>
        private void SetupMockFilesystemToReadDataset()
        {
            this._mockFileSystem
                .Setup(i => i.File.Exists(It.IsAny<string>()))
                .Returns(true);

            this._mockFileSystem
                .Setup(i => i.Path.Join(It.Is<string>(s => s == "./Res"), It.IsAny<string>()))
                .Returns("./Res/World Atlas.tif");
        }

        /// <summary>
        /// Initialises a new intance of the <see cref="ILightPollutionService"/> class for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new LightPollutionService(this._mockFileSystem.Object);
        }
    }
}
