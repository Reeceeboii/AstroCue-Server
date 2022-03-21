namespace AstroCue.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
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
    /// Tests targeting <see cref="MappingService"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MappingServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private IMappingService _sut;

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
        /// Fake MapBox API key
        /// </summary>
        private const string FakeMapBoxApiKey = "B76C88D4-AEC5-46B2-9FBA-CFEB505173B0";

        /// <summary>
        /// Fake MapBox fwd geocode base URL used by the tests
        /// </summary>
        private const string FakeFwdGeocodeBaseUrl = "https://localhost/";

        /// <summary>
        /// Fake MapBox fwd geocode resources URL used by the tests
        /// </summary>
        private const string FakeFwdGeocodeResourceUrl = "https://localhost/test.json";

        /// <summary>
        /// Fake MapBox static map image base URL used by the tests
        /// </summary>
        private const string FakeStaticMapImagesBaseUrl = "https://localhost/";

        /// <summary>
        /// Initialises a new instance of the <see cref="MappingServiceTests"/> class
        /// </summary>
        public MappingServiceTests()
        {
            this._mockEnvironmentManager = new Mock<IEnvironmentManager>();
            this._mockHttpClientService = new Mock<IHttpClientService>();

            this._mockHttpMessageHandler = new MockHttpMessageHandler();

            RestClientOptions opts = new()
            {
                ConfigureMessageHandler = _ => this._mockHttpMessageHandler
            };

            HttpClient client = this._mockHttpMessageHandler.ToHttpClient();
            client.BaseAddress = new Uri(FakeFwdGeocodeBaseUrl);

            this._mockHttpClientService
                .Setup(i => i.NewClient())
                .Returns(new RestClient(client, opts));

            this._mockEnvironmentManager
                .Setup(i => i.MapBoxApiKey)
                .Returns(FakeMapBoxApiKey);

            this._mockEnvironmentManager
                .Setup(i => i.BaseMapBoxForwardGeocodeUrl)
                .Returns(FakeFwdGeocodeBaseUrl);

            this._mockEnvironmentManager
                .Setup(i => i.BaseMapBoxStaticMapsUrl)
                .Returns(FakeStaticMapImagesBaseUrl);
        }

        /// <summary>
        /// Tests that the <see cref="MappingService"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseMappingServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that the forward geocode queries are validated against the MapBox requirements before
        /// they are used
        /// </summary>
        [TestMethod]
        public void ForwardGeocodeQueryValidationTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            
            // queries > 20 words not allowed
            Assert.ThrowsException<AggregateException>(() =>
                this._sut.ForwardGeocodeAsync(string.Concat(Enumerable.Repeat("word ", 21))).Result);

            // semi colons not allowed in queries
            Assert.ThrowsException<AggregateException>(() =>
                this._sut.ForwardGeocodeAsync("some query with illegal character ;").Result);
        }

        /// <summary>
        /// Tests that forward geocoding works as expected
        /// </summary>
        [TestMethod]
        public void ForwardGeocodeTest()
        {
            // Arrange
            MockedRequest mockedRequest = this._mockHttpMessageHandler
                .When(FakeFwdGeocodeResourceUrl)
                .WithExactQueryString(new Dictionary<string, string>()
                {
                    { "autocomplete", "True" },
                    { "access_token", FakeMapBoxApiKey }
                })
                .Respond("application/json", TestData.MapBoxForwardGeocodeSample);

            // Act
            this.CreateSut();
            IList<FwdGeocodeResult> res = this._sut.ForwardGeocodeAsync("test").Result;

            // Assert
            this._mockHttpMessageHandler.GetMatchCount(mockedRequest).Should().Be(1);

            // data defined in TestData.cs class
            res.Count.Should().Be(1);
            res[0].Text.Should().Be("Test 1");
            res[0].PlaceName.Should().Be("Test address 1");
            res[0].Longitude.Should().Be(-0.102327f);
            res[0].Latitude.Should().Be(51.527388f);
        }

        /// <summary>
        /// Tests that retrieving static map images works as expected
        /// </summary>
        [TestMethod]
        public void GetStaticMapImageTest()
        {
            // Arrange
            const float testLongitude = 1f;
            const float testLatitude = testLongitude;

            byte[] imageByteStream = Encoding.ASCII.GetBytes("In the real world, this will be image data!");

            string fakeResourcesUrl =
                $"{FakeStaticMapImagesBaseUrl}pin-s+999({testLongitude},{testLatitude})/{testLongitude},{testLatitude},12.5,0/400x275@2x";

            MockedRequest mockedRequest = this._mockHttpMessageHandler
                .When(fakeResourcesUrl)
                .WithExactQueryString(new Dictionary<string, string>()
                {
                    { "access_token", FakeMapBoxApiKey }
                })
                .Respond("image/png", new MemoryStream(imageByteStream));

            // Act
            this.CreateSut();
            byte[] image = this._sut.GetStaticMapImageAsync(testLongitude, testLatitude).Result;

            // Assert
            image.Length.Should().Be(imageByteStream.Length);
            image.SequenceEqual(imageByteStream).Should().BeTrue();
        }

        /// <summary>
        /// Initialises a new <see cref="MappingService"/> instance for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new MappingService(this._mockEnvironmentManager.Object, this._mockHttpClientService.Object);
        }
    }
}
