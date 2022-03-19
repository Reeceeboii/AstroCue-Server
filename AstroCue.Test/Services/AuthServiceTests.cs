namespace AstroCue.Test.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Data;
    using Server.Entities;
    using Server.Services;
    using Server.Services.Interfaces;

    /// <summary>
    /// Tests targeting <see cref="AuthService"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AuthServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private AuthService _sut;

        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/> used by tests
        /// </summary>
        private readonly ApplicationDbContext _inMemoryDbContext;

        /// <summary>
        /// Mock <see cref="IEmailService"/> used by the tests
        /// </summary>
        private readonly Mock<IEmailService> _mockEmailService;

        /// <summary>
        /// Test <see cref="AstroCueUser"/> used by the tests
        /// </summary>
        private readonly AstroCueUser _testUser = new ()
        {
            FirstName = "Joe",
            LastName = "Bloggs",
            EmailAddress = "example@test.com"
        };

        /// <summary>
        /// Initialises a new instance of the <see cref="AuthServiceTests"/> class
        /// </summary>
        public AuthServiceTests()
        {
            this._inMemoryDbContext = TestUtilities.InMemoryDatabase.NewInMemoryDbContext();
            this._mockEmailService = new Mock<IEmailService>();
        }

        /// <summary>
        /// Tests that the <see cref="AuthService"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseAuthServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a new user can be registered
        /// </summary>
        [TestMethod]
        public void RegisterAstroCueUserTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser reg = this._sut.RegisterAstroCueUser(this._testUser, "password");

            // Assert
            reg.Should().NotBeNull();
            reg.Id.Should().NotBe(0);
            reg.FirstName.Should().Be(this._testUser.FirstName);
            reg.LastName.Should().Be(this._testUser.LastName);
            reg.EmailAddress.Should().Be(this._testUser.EmailAddress);
            reg.PasswordHash.Length.Should().BeGreaterThan(1);
            reg.PasswordSalt.Length.Should().BeGreaterThan(1);

            this._mockEmailService
                .Verify(m => m.SendWelcomeEmail(
                    It.Is<AstroCueUser>(u => u.EmailAddress == this._testUser.EmailAddress)), Times.Once());
        }

        /// <summary>
        /// Tests that a new user can be registered and that their data is cleaned
        /// during the registration process
        /// </summary>
        [TestMethod]
        public void RegisterAstroCueUserBadDataTest()
        {
            // Arrange
            AstroCueUser testUser = new()
            {
                FirstName = " joe",
                LastName = " bloGGs     ",
                EmailAddress = "  ExamplE@tEst.cOm  "
            };

            // Act
            this.CreateSut();
            AstroCueUser reg = this._sut.RegisterAstroCueUser(testUser, "password");

            // Assert
            reg.Should().NotBeNull();
            reg.Id.Should().NotBe(0);
            reg.FirstName.Should().Be("Joe");
            reg.LastName.Should().Be("Bloggs");
            reg.EmailAddress.Should().Be("example@test.com");
            reg.PasswordHash.Length.Should().BeGreaterThan(1);
            reg.PasswordSalt.Length.Should().BeGreaterThan(1);


            this._mockEmailService
                .Verify(m => m.SendWelcomeEmail(
                    It.Is<AstroCueUser>(u => u.EmailAddress == "example@test.com")), Times.Once());
        }

        /// <summary>
        /// Tests that a new user can't be registered without a password
        /// </summary>
        [TestMethod]
        public void RegisterNoPasswordTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            ArgumentException nullErr = Assert.ThrowsException<ArgumentException>(
                () => this._sut.RegisterAstroCueUser(this._testUser, null));

            ArgumentException emptyErr = Assert.ThrowsException<ArgumentException>(
                () => this._sut.RegisterAstroCueUser(this._testUser, string.Empty));

            ArgumentException whitespaceErr = Assert.ThrowsException<ArgumentException>(
                () => this._sut.RegisterAstroCueUser(this._testUser, "    "));

            nullErr.Message.Should().Be("Password needs to be present");
            emptyErr.Message.Should().Be("Password needs to be present");
            whitespaceErr.Message.Should().Be("Password needs to be present");
        }

        /// <summary>
        /// Tests that an email can't be registered twice
        /// </summary>
        [TestMethod]
        public void RegisterExistingEmailTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser reg = this._sut.RegisterAstroCueUser(this._testUser, "password");

            // Assert
            reg.Should().NotBeNull();
            ArgumentException ex = Assert.ThrowsException<ArgumentException>(
                () => this._sut.RegisterAstroCueUser(this._testUser, "password"));

            ex.Message.Should().Be($"An account with email {this._testUser.EmailAddress} already exists");
        }

        /// <summary>
        /// Tests that a user can be authenticated
        /// </summary>
        [TestMethod]
        public void AuthenticateAstroCueUserTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser reg = this._sut.RegisterAstroCueUser(this._testUser, "password");
            AstroCueUser authed = this._sut.AuthenticateAstroCueUser(this._testUser.EmailAddress, "password");

            // Assert
            reg.Should().NotBeNull();
            authed.Should().NotBeNull();
            authed.Id.Should().NotBe(0);

            authed.EmailAddress.Should().Be(reg.EmailAddress);
            authed.FirstName.Should().Be(reg.FirstName);
            authed.LastName.Should().Be(reg.LastName);
        }

        /// <summary>
        /// Tests that you can't authenticate with a blank or null email or password
        /// </summary>
        [TestMethod]
        public void AuthenticateAstroCueUserBlankCredentialsTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser nullEmailTest = this._sut.AuthenticateAstroCueUser(null, null);
            AstroCueUser emptyEmailTest = this._sut.AuthenticateAstroCueUser(string.Empty, null);
            AstroCueUser whitespaceEmailTest = this._sut.AuthenticateAstroCueUser("     ", null);
            AstroCueUser nullPasswordTest = this._sut.AuthenticateAstroCueUser(null, null);
            AstroCueUser emptyPasswordTest = this._sut.AuthenticateAstroCueUser(null, string.Empty);
            AstroCueUser whitespacePasswordTest = this._sut.AuthenticateAstroCueUser(null, "     ");

            // Assert
            nullEmailTest.Should().BeNull();
            emptyEmailTest.Should().BeNull();
            whitespaceEmailTest.Should().BeNull();
            nullPasswordTest.Should().BeNull();
            emptyPasswordTest.Should().BeNull();
            whitespacePasswordTest.Should().BeNull();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="AuthService"/> for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new AuthService(this._inMemoryDbContext, this._mockEmailService.Object);
        }
    }
}
