namespace AstroCue.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using RestSharp;
    using RichardSzalay.MockHttp;
    using Server.Data.Interfaces;
    using Server.Models.Misc;
    using Server.Services;
    using Server.Services.Interfaces;
    using TestUtilities;

    /// <summary>
    /// Tests targeting <see cref="WeatherForecastService"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WeatherForecastServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private IWeatherForecastService _sut;

        /// <summary>
        /// Mock <see cref="IEnvironmentManager"/> used in tests
        /// </summary>
        private readonly Mock<IEnvironmentManager> _mockEnvironmentManager;

        /// <summary>
        /// Mock <see cref="IHttpClientService"/> used in tests
        /// </summary>
        private readonly Mock<IHttpClientService> _mockHttpClientService;

        /// <summary>
        /// Mock HttpMessageHandler used by the tests
        /// </summary>
        private readonly MockHttpMessageHandler _mockHttpMessageHandler;

        /// <summary>
        /// Fake OWM API key
        /// </summary>
        private const string FakeOwmApiKey = "B76C88D4-AEC5-46B2-9FBA-CFEB505173B0";

        /// <summary>
        /// Fake OWM current weather base URL used by the tests
        /// </summary>
        private const string FakeCurrentWeatherUrl = "https://localhost/";

        /// <summary>
        /// Initialises a new instance of the <see cref="WeatherForecastServiceTests"/> class
        /// </summary>
        public WeatherForecastServiceTests()
        {
            this._mockEnvironmentManager = new Mock<IEnvironmentManager>();
            this._mockHttpClientService = new Mock<IHttpClientService>();

            this._mockHttpMessageHandler = new MockHttpMessageHandler();

            RestClientOptions opts = new()
            {
                ConfigureMessageHandler = _ => this._mockHttpMessageHandler
            };

            HttpClient client = this._mockHttpMessageHandler.ToHttpClient();
            client.BaseAddress = new Uri(FakeCurrentWeatherUrl);

            this._mockHttpClientService
                .Setup(i => i.NewClient())
                .Returns(new RestClient(client, opts));

            this._mockEnvironmentManager
                .Setup(i => i.OpenWeatherMapApiKey)
                .Returns(FakeOwmApiKey);

            this._mockEnvironmentManager
                .Setup(i => i.BaseOwmCurrentWeatherUrl)
                .Returns(FakeCurrentWeatherUrl);
        }

        /// <summary>
        /// Tests that the <see cref="WeatherForecastService"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseWeatherForecastServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the <see cref="WeatherForecastService.GetCurrentWeatherAsync"/> method
        /// </summary>
        [TestMethod]
        public void GetCurrentWeatherAsyncTest()
        {
            // Arrange
            const float testLongitude = -0.102327f;
            const float testLatitude = -51.527388f;

            MockedRequest mockedRequest = this._mockHttpMessageHandler
                .When(FakeCurrentWeatherUrl)
                .WithExactQueryString(new Dictionary<string, string>()
                {
                    { "lat", $"{testLatitude}" },
                    { "lon", $"{testLongitude}" },
                    { "units", "metric" },
                    { "appid", FakeOwmApiKey }
                })
                .Respond("application/json", TestData.OwmCurrentWeatherSample);

            // Act
            this.CreateSut();
            SingleForecast res = this._sut.GetCurrentWeatherAsync(testLongitude, testLatitude).Result;

            // Assert
            res.Should().NotBeNull();
            this._mockHttpMessageHandler.GetMatchCount(mockedRequest).Should().Be(1);
            res.Description.Should().Be("Overcast clouds");
            res.CloudCoveragePercent.Should().Be(92);
            res.RetrievedAt.Should().Be(DateTime.Now.ToString("s"));
        }

        /// <summary>
        /// Initialises a new instance of <see cref="WeatherForecastService"/> to be used in tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new WeatherForecastService(this._mockEnvironmentManager.Object,
                this._mockHttpClientService.Object);
        }
    }
}