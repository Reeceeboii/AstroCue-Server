namespace AstroCue.Test.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Astronomy;
    using Server.Controllers;
    using Server.Controllers.Parameters;
    using Server.Models.API.Inbound;
    using Server.Models.API.Outbound;
    using Server.Models.Misc;
    using Server.Services;
    using Server.Services.Interfaces;
    using TestUtilities;

    /// <summary>
    /// Tests targeting <see cref="ObservationLocationController"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ObservationLocationControllerTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private ObservationLocationController _sut;

        /// <summary>
        /// Mock <see cref="ObservationLocationService"/> used by the tests
        /// </summary>
        private readonly Mock<IObservationLocationService> _mockObservationLocationService;

        /// <summary>
        /// Mock <see cref="IMapper"/> used by the tests
        /// </summary>
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObservationLocationControllerTests"/> class
        /// </summary>
        public ObservationLocationControllerTests()
        {
            this._mockObservationLocationService = new Mock<IObservationLocationService>();
            this._mockMapper = new Mock<IMapper>();
        }

        /// <summary>
        /// Tests that the <see cref="ObservationLocationController"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseObservationLocationControllerTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the <see cref="ObservationLocationController.AddNewObservationLocation"/> endpoint
        /// </summary>
        [TestMethod]
        public void AddNewObservationLocationTest()
        {
            // Arrange
            this.CreateSut();
            ControllerUtilities.SetHttpContextUserId(this._sut, 1);

            InboundObsLocationModel inboundObs = new()
            {
                Name = "Test location",
                Longitude = 5f,
                Latitude = 5f
            };

            OutboundObsLocationModel outboundObs = new()
            {
                Id = 1,
                Latitude = inboundObs.Latitude,
                Longitude = inboundObs.Longitude,
                Name = inboundObs.Name,
                BortleScaleValue = 3,
                BortleDesc = BortleScale.ScaleToDescription(3),
                SingleForecast = null
            };

            this._mockObservationLocationService
                .Setup(i =>
                    i.AddNew(1, It.Is<InboundObsLocationModel>(l 
                        => l.Name == inboundObs.Name)))
                .Returns(outboundObs);

            // Act
            IActionResult res = this._sut.AddNewObservationLocation(inboundObs);

            // Assert
            res.Should().NotBeNull();
            OkObjectResult resObj = (OkObjectResult)res;
            resObj.StatusCode.Should().Be(StatusCodes.Status200OK);
            OutboundObsLocationModel outboundRes = (OutboundObsLocationModel)resObj.Value;

            outboundRes.Id.Should().Be(outboundObs.Id);
            outboundRes.Latitude.Should().Be(outboundObs.Latitude);
            outboundRes.Longitude.Should().Be(outboundObs.Longitude);
            outboundRes.Name.Should().Be(outboundObs.Name);
            outboundRes.BortleScaleValue.Should().Be(outboundObs.BortleScaleValue);
            outboundRes.BortleDesc.Should().Be(outboundObs.BortleDesc);
            outboundRes.SingleForecast.Should().BeNull();
        }

        /// <summary>
        /// Tests the <see cref="ObservationLocationController.AddNewObservationLocation"/> endpoint
        /// returning a 500 internal server error
        /// </summary>
        [TestMethod]
        public void AddNewObservationLocationServerErrorTest()
        {
            // Arrange
            this.CreateSut();
            ControllerUtilities.SetHttpContextUserId(this._sut, 1);

            InboundObsLocationModel inboundObs = new()
            {
                Name = "Test location",
                Longitude = 5f,
                Latitude = 5f
            };

            this._mockObservationLocationService
                .Setup(i =>
                    i.AddNew(1, It.Is<InboundObsLocationModel>(l
                        => l.Name == inboundObs.Name)))
                .Returns((OutboundObsLocationModel)null);

            // Act
            IActionResult res = this._sut.AddNewObservationLocation(inboundObs);

            // Assert
            res.Should().NotBeNull();
            ObjectResult resObj = (ObjectResult)res;
            resObj.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            OutboundErrorModel outboundRes = (OutboundErrorModel)resObj.Value;
            outboundRes.Should().NotBeNull();
            outboundRes.Message.Should().Be("Failed to add location to your account, please try again");
        }

        /// <summary>
        /// Tests the <see cref="ObservationLocationController.DeleteObservationLocation"/> endpoint
        /// </summary>
        [TestMethod]
        public void DeleteObservationLocationTest()
        {
            // Arrange
            this.CreateSut();
            ControllerUtilities.SetHttpContextUserId(this._sut, 1);

            IdParameter id = new()
            {
                Id = 1
            };

            this._mockObservationLocationService
                .Setup(i =>
                    i.Delete(1, It.Is<int>(l
                        => l == id.Id)))
                .Returns(new OutboundObsLocationModel()
                {
                    Name = "Deleted location"
                });

            // Act
            IActionResult res = this._sut.DeleteObservationLocation(id);
            res.Should().NotBeNull();
            OkObjectResult okObj = (OkObjectResult)res;
            okObj.StatusCode.Should().Be(StatusCodes.Status200OK);
            OutboundObsLocationModel outboundObs = (OutboundObsLocationModel)okObj.Value;
            outboundObs.Should().NotBeNull();
            outboundObs.Name.Should().Be("Deleted location");
        }

        /// <summary>
        /// Tests the <see cref="ObservationLocationController.GetAllObservationLocations"/> endpoint
        /// </summary>
        [TestMethod]
        public void GetAllObservationLocationsTest()
        {
            // Arrange
            this.CreateSut();
            ControllerUtilities.SetHttpContextUserId(this._sut, 1);

            List<OutboundObsLocationModel> result = new()
            {
                new OutboundObsLocationModel
                {
                    Id = 1,
                    Name = "Location 1",
                    Longitude = 5f,
                    Latitude = 5f,
                    BortleScaleValue = 4,
                    BortleDesc = BortleScale.ScaleToDescription(4),
                    SingleForecast = new SingleForecast()
                },
                new OutboundObsLocationModel
                {
                    Id = 2,
                    Name = "Location 2",
                    Longitude = 5f,
                    Latitude = 5f,
                    BortleScaleValue = 1,
                    BortleDesc = BortleScale.ScaleToDescription(1),
                    SingleForecast = new SingleForecast()
                }
            };

            this._mockObservationLocationService
                .Setup(i => i.GetAllAsync(It.Is<int>(id => id == 1)))
                .ReturnsAsync(result);

            // Act
            ActionResult<List<OutboundObsLocationModel>> res =
                this._sut.GetAllObservationLocations().Result;

            // Assert
            res.Should().NotBeNull();
            OkObjectResult okObj = (OkObjectResult)res.Result;
            okObj.StatusCode.Should().Be(StatusCodes.Status200OK);
            List<OutboundObsLocationModel> locs = (List<OutboundObsLocationModel>)okObj.Value;
            locs.Count.Should().Be(result.Count);
            locs[0].Should().Be(result[0]);
            locs[1].Should().Be(result[1]);
        }

        /// <summary>
        /// Tests the <see cref="ObservationLocationController.GetStaticMap"/> endpoint
        /// </summary>
        [TestMethod]
        public void GetStaticMapTest()
        {
            // Arrange
            this.CreateSut();
            ControllerUtilities.SetHttpContextUserId(this._sut, 1);

            byte[] image = Encoding.ASCII.GetBytes("In the real world this will be image data!");

            this._mockObservationLocationService
                .Setup(i => i.GetStaticMapAsync(1, 1))
                .ReturnsAsync(image);

            // Act
            IActionResult res = this._sut.GetStaticMap(1, false).Result;

            // Assert
            res.Should().NotBeNull();
            FileContentResult file = (FileContentResult)res;
            file.ContentType.Should().Be("image/png");
            file.FileContents.Length.Should().Be(image.Length);
        }

        /// <summary>
        /// Create a new instance of <see cref="ObservationLocationController"/> to be used in tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new ObservationLocationController(
                this._mockObservationLocationService.Object,
                this._mockMapper.Object);
        }
    }
}
