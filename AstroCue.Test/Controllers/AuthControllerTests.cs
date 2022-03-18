namespace AstroCue.Test.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Controllers;
    using Server.Data.Interfaces;
    using Server.Entities;
    using Server.Models.API.Inbound;
    using Server.Models.API.Outbound;
    using Server.Services.Interfaces;

    /// <summary>
    /// Class for testing <see cref="AuthController"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthControllerTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private AuthController _sut;
        
        /// <summary>
        /// A mock of <see cref="IAuthService"/> used by the tests
        /// </summary>
        private readonly Mock<IAuthService> _mockAuthService;

        /// <summary>
        /// A mock of <see cref="IAstroCueUserService"/> used by the tests
        /// </summary>
        private readonly Mock<IAstroCueUserService> _mockAstroCueUserService;

        /// <summary>
        /// A mock <see cref="IMapper"/> used by the tests
        /// </summary>
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Mock <see cref="IEnvironmentManager"/> used by the tests
        /// </summary>
        private readonly Mock<IEnvironmentManager> _mockEnvironmentManager;

        /// <summary>
        /// <see cref="InboundRegModel"/> used by the tests
        /// </summary>
        private readonly InboundRegModel _testInboundRegModel = new()
        {
            FirstName = "Joe",
            LastName = "Bloggs",
            EmailAddress = "example@test.com",
            Password = "password"
        };

        /// <summary>
        /// <see cref="InboundAuthModel"/> used by the tests
        /// </summary>
        private readonly InboundAuthModel _testInboundAuthModel = new()
        {
            EmailAddress = "example@test.com",
            Password = "password"
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="AuthControllerTests"/> class
        /// </summary>
        public AuthControllerTests()
        {
            this._mockAuthService = new Mock<IAuthService>();
            this._mockAstroCueUserService = new Mock<IAstroCueUserService>();
            this._mockMapper = new Mock<IMapper>();
            this._mockEnvironmentManager = new Mock<IEnvironmentManager>();
        }

        /// <summary>
        /// Tests that <see cref="AuthController"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseAuthControllerTest()
        {
            // Arrange
            this.CreateSut();

            // Act
            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a new user can be reigstered succesfully
        /// </summary>
        [TestMethod]
        public void CanRegisterNewUserTest()
        {
            // Arrange
            this._mockAuthService
                .Setup(i => i.RegisterAstroCueUser(It.IsAny<AstroCueUser>(), It.IsAny<string>()))
                .Returns(new AstroCueUser());

            this.CreateSut();

            // Act
            IActionResult response = this._sut.Register(this._testInboundRegModel);

            // Assert
            this._mockAuthService.Verify(
                m => m.RegisterAstroCueUser(It.IsAny<AstroCueUser>(),
                    It.Is<string>(s => s == this._testInboundRegModel.Password)), Times.Once());

            this._mockMapper.Verify(m => m.Map<AstroCueUser>(It.IsAny<InboundRegModel>()), Times.Once());

            response.Should().BeOfType<OkResult>();
        }

        /// <summary>
        /// Tests that the controller returns an error message when the registration fails for whatever reason
        /// </summary>
        [TestMethod]
        public void RegistrationErrorMessageTest()
        {
            // Arrange
            const string testErrorMessage ="Some error thrown from service layer";

            this._mockAuthService
                .Setup(i => i.RegisterAstroCueUser(It.IsAny<AstroCueUser>(), It.IsAny<string>()))
                .Throws(new Exception(testErrorMessage));

            this.CreateSut();

            // Act
            IActionResult response = this._sut.Register(this._testInboundRegModel);

            // Assert
            this._mockAuthService.Verify(
                m => m.RegisterAstroCueUser(It.IsAny<AstroCueUser>(),
                    It.Is<string>(s => s == this._testInboundRegModel.Password)), Times.Once());

            response.Should().BeOfType<BadRequestObjectResult>();

            this._mockMapper.Verify(m => m.Map<AstroCueUser>(It.IsAny<InboundRegModel>()), Times.Once());

            BadRequestObjectResult res = response as BadRequestObjectResult;
            OutboundErrorModel actualError = res!.Value as OutboundErrorModel;

            actualError!.Message.Should().Be(testErrorMessage);
        }

        /// <summary>
        /// Tests that a user can be logged in and a JWT is generated for them
        /// </summary>
        [TestMethod]
        public void CanLoginUserTest()
        {
            // Arrange
            OutboundAuthSuccessModel testOutboundAuthSuccessModel = new()
            {
                Id = 1,
                EmailAddress = this._testInboundAuthModel.EmailAddress,
                FirstName = "Joe",
                LastName = "Bloggs"
            };

            this._mockEnvironmentManager.Setup(i => i.JwtSecret).Returns("test_secret_for_testing_type_purposes");

            this._mockMapper
                .Setup(m => m.Map<OutboundAuthSuccessModel>(It.IsAny<AstroCueUser>()))
                .Returns(testOutboundAuthSuccessModel);

            this._mockAuthService
                .Setup(i => i.AuthenticateAstroCueUser(
                It.Is<string>(s => s == this._testInboundAuthModel.EmailAddress),
                It.Is<string>(s => s == this._testInboundAuthModel.Password)))
                .Returns(new AstroCueUser());

            // Act
            this.CreateSut();
            IActionResult response = this._sut.Login(this._testInboundAuthModel);

            // Assert
            response.Should().BeOfType<OkObjectResult>();

            this._mockMapper.Verify(m => m.Map<OutboundAuthSuccessModel>(It.IsAny<AstroCueUser>()), Times.Once());

            OkObjectResult res = response as OkObjectResult;
            OutboundAuthSuccessModel model = res!.Value as OutboundAuthSuccessModel;

            model!.Id.Should().Be(testOutboundAuthSuccessModel.Id);
            model!.EmailAddress.Should().Be(testOutboundAuthSuccessModel.EmailAddress);
            model!.FirstName.Should().Be(testOutboundAuthSuccessModel.FirstName);
            model!.LastName.Should().Be(testOutboundAuthSuccessModel.LastName);
            model!.Token.Length.Should().BeGreaterOrEqualTo(1);
        }

        /// <summary>
        /// Tests that the controller returns an error message when the registration fails for whatever reason
        /// </summary>
        [TestMethod]
        public void LoginErrorTest()
        {
            // Arrange
            this._mockAuthService
                .Setup(i => i.AuthenticateAstroCueUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((AstroCueUser)null);

            // Act
            this.CreateSut();
            IActionResult response = this._sut.Login(new InboundAuthModel());

            // Assert
            response.Should().BeOfType<ObjectResult>();

            ObjectResult res = response as ObjectResult;
            OutboundErrorModel model = res!.Value as OutboundErrorModel;

            model!.Message.Should().Be("Your email or password is incorrect");
        }

        /// <summary>
        /// Initialises a new <see cref="AuthController"/> instance to be used by the tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new AuthController(
                this._mockAuthService.Object,
                this._mockAstroCueUserService.Object,
                this._mockMapper.Object,
                this._mockEnvironmentManager.Object);
        }
    }
}
