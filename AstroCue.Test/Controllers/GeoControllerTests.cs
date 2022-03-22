namespace AstroCue.Test.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Controllers;
    using Server.Controllers.Parameters;
    using Server.Models.Misc;
    using Server.Services.Interfaces;

    /// <summary>
    /// Tests targeting <see cref="GeoController"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GeoControllerTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private GeoController _sut;

        /// <summary>
        /// Mock <see cref="IMappingService"/> used by the tests
        /// </summary>
        private readonly Mock<IMappingService> _mockMappingService;

        /// <summary>
        /// Initialise a new instance of the <see cref="GeoControllerTests"/> class
        /// </summary>
        public GeoControllerTests()
        {
            this._mockMappingService = new Mock<IMappingService>();
        }

        [TestMethod]
        public void CanInitialiseGeoControllerTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        [TestMethod]
        public void ForwardGeocodeTest()
        {
            // Arrange
            const float testLatitude = 50.123345f;
            const float testLongitude = -0.43377f;
            const string testPlaceName = "test";
            const string testText = "test location";

            this._mockMappingService
                .Setup(i => i.ForwardGeocodeAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<FwdGeocodeResult>()
                {
                    new()
                    {
                        Latitude = testLatitude,
                        Longitude = testLongitude,
                        PlaceName = testPlaceName,
                        Text = testText
                    }
                });

            // Act
            this.CreateSut();
            ActionResult<IList<FwdGeocodeResult>> res = this._sut.ForwardGeocode(new FwdGeocodeParams
            {
                Query = "test"
            }).Result;

            // Assert
            this._mockMappingService.Verify(m => m.ForwardGeocodeAsync(It.IsAny<string>()), Times.Once());
            res.Should().NotBeNull();

            OkObjectResult okObj = (OkObjectResult)res.Result;
            List<FwdGeocodeResult> results = okObj.Value as List<FwdGeocodeResult>;
            results.Should().NotBeNull();

            results!.Count.Should().Be(1);
            results![0].Longitude.Should().Be(testLongitude);
            results![0].Latitude.Should().Be(testLatitude);
            results![0].PlaceName.Should().Be(testPlaceName);
            results![0].Text.Should().Be(testText);
        }

        /// <summary>
        /// Tests that the correct error is returned when the forward geocode query is too long
        /// </summary>
        [TestMethod]
        public void QueryTooLongTest()
        {
            // Arrange
            const string query = "test";
            this._mockMappingService
                .Setup(i => i.ForwardGeocodeAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentOutOfRangeException(nameof(query), "test"));

            // Act
            this.CreateSut();
            ActionResult<IList<FwdGeocodeResult>> response = this._sut.ForwardGeocode(new FwdGeocodeParams
            {
                Query = "test"
            }).Result;

            // Assert
            this._mockMappingService.Verify(m => m.ForwardGeocodeAsync(It.IsAny<string>()), Times.Once());
            response.Should().NotBeNull();

            response.Result.Should().BeOfType<ObjectResult>();
            ObjectResult res = (ObjectResult)response.Result;
            res.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Tests that the correct error is returned when the forward geocode query contains an illegal character
        /// </summary>
        [TestMethod]
        public void QueryContainsIllegalCharactestTest()
        {
            // Arrange
            this._mockMappingService
                .Setup(i => i.ForwardGeocodeAsync(It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("test"));

            // Act
            this.CreateSut();
            ActionResult<IList<FwdGeocodeResult>> response = this._sut.ForwardGeocode(new FwdGeocodeParams
            {
                Query = "test"
            }).Result;

            // Assert
            this._mockMappingService.Verify(m => m.ForwardGeocodeAsync(It.IsAny<string>()), Times.Once());
            response.Should().NotBeNull();

            response.Result.Should().BeOfType<ObjectResult>();
            ObjectResult res = (ObjectResult)response.Result;
            res.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Tests that server errors are caught
        /// </summary>
        [TestMethod]
        public void ServerErrorTest()
        {
            // Arrange
            this._mockMappingService
                .Setup(i => i.ForwardGeocodeAsync(It.IsAny<string>()))
                .ThrowsAsync(new NullReferenceException());

            // Act
            this.CreateSut();
            ActionResult<IList<FwdGeocodeResult>> response = this._sut.ForwardGeocode(new FwdGeocodeParams
            {
                Query = "test"
            }).Result;

            // Assert
            this._mockMappingService.Verify(m => m.ForwardGeocodeAsync(It.IsAny<string>()), Times.Once());
            response.Should().NotBeNull();

            response.Result.Should().BeOfType<ObjectResult>();
            ObjectResult res = (ObjectResult)response.Result;
            res.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Create a new <see cref="GeoController"/> instance to be used in tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new GeoController(this._mockMappingService.Object);
        }
    }
}
