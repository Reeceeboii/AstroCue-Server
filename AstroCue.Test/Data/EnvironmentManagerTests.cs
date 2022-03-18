namespace AstroCue.Test.Data
{
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Server.Data;
    using Server.Data.Interfaces;

    /// <summary>
    /// Tests targeting <see cref="Server.Data.EnvironmentManager"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EnvironmentManagerTests
    {
        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/> used in tests
        /// </summary>
        private IEnvironmentManager _sut;

        /// <summary>
        /// Mock <see cref="IConfiguration"/> used in tests
        /// </summary>
        private readonly Mock<IConfiguration> _mockConfiguration;

        /// <summary>
        /// JWT key index
        /// </summary>
        private const string JsonWebTokenSecretIndex = "AppSettings:JsonWebTokenSecret";

        /// <summary>
        /// JWT key value
        /// </summary>
        private const string JsonWebTokenSecretValue = "some_key";

        /// <summary>
        /// MapBox API key index
        /// </summary>
        private const string MapBoxApiKeyIndex = "MapBox:APIKey";

        /// <summary>
        /// MapBox API key value
        /// </summary>
        private const string MapBoxApiKeyValue = "mapboxapikey";

        /// <summary>
        /// OpenWeatherMap API key index
        /// </summary>
        private const string OpenWeatherMapApiKeyIndex = "OpenWeatherMap:APIKey";

        /// <summary>
        /// OpenWeatherMap API key value
        /// </summary>
        private const string OpenWeatherMapApiKeyValue = "owmapikey";

        /// <summary>
        /// MailGun API key index
        /// </summary>
        private const string MailGunApiKeyIndex = "MailGun:APIKey";

        /// <summary>
        /// MailGun API key value
        /// </summary>
        private const string MailGunApiKeyValue = "mailgunkey";

        /// <summary>
        /// Base MailGun messages URL
        /// </summary>
        private const string BaseMailGunMessagesUrl = "https://api.eu.mailgun.net/v3";

        /// <summary>
        /// Initialises a new instance of the <see cref="EnvironmentManagerTests"/> class
        /// </summary>
        public EnvironmentManagerTests()
        {
            this._mockConfiguration = new Mock<IConfiguration>();

            // JWT
            this._mockConfiguration
                .Setup(i => i[It.Is<string>(s => s == JsonWebTokenSecretIndex)])
                .Returns(JsonWebTokenSecretValue);

            // MapBox API key
            this._mockConfiguration
                .Setup(i => i[It.Is<string>(s => s == MapBoxApiKeyIndex)])
                .Returns(MapBoxApiKeyValue);

            // OpenWeatherMap API key
            this._mockConfiguration
                .Setup(i => i[It.Is<string>(s => s == OpenWeatherMapApiKeyIndex)])
                .Returns(OpenWeatherMapApiKeyValue);

            // MailGun API key
            this._mockConfiguration
                .Setup(i => i[It.Is<string>(s => s == MailGunApiKeyIndex)])
                .Returns(MailGunApiKeyValue);
        }

        /// <summary>
        /// Test that the <see cref="EnvironmentManager"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseEnvironmentManagerTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();

            this._sut.JwtSecret.Should().Be(JsonWebTokenSecretValue);
            this._sut.MapBoxApiKey.Should().Be(MapBoxApiKeyValue);
            this._sut.MailGunApiKey.Should().Be(MailGunApiKeyValue);
            this._sut.OpenWeatherMapApiKey.Should().Be(OpenWeatherMapApiKeyValue);
            this._sut.BaseMailGunMessagesUrl.Should().Be(BaseMailGunMessagesUrl);
        }

        /// <summary>
        /// Initialises a new instance of <see cref="IEnvironmentManager"/> used in tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new EnvironmentManager(this._mockConfiguration.Object);
        }
    }
}
