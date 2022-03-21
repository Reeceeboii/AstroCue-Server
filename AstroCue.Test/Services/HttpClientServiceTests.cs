namespace AstroCue.Test.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RestSharp;
    using Server.Services;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpClientServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private HttpClientService _sut;

        /// <summary>
        /// Tests that the <see cref="HttpClientService"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialiseHttpClientServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the <see cref="HttpClientService.NewClientBasicAuth"/> method
        /// </summary>
        [TestMethod]
        public void CanMakeNewClientWithBasicAuthTest()
        {
            // Arrange
            const string testApiKey = "12345";
            const string testBaseUrl = "https://localhost:5000";

            // Act
            this.CreateSut();
            RestClient client = this._sut.NewClientBasicAuth(testApiKey, testBaseUrl);

            // Assert
            client.Should().NotBeNull();
            client.Authenticator.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that errors are thrown from missing parameters
        /// </summary>
        [TestMethod]
        public void NewClientWithBasicAuthMissingParametersTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            Assert.ThrowsException<UriFormatException>(() => this._sut.NewClientBasicAuth(string.Empty, string.Empty));
        }

        /// <summary>
        /// Tests that a plain new <see cref="RestClient"/> instance can be created
        /// </summary>
        [TestMethod]
        public void NewClientTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            RestClient client = this._sut.NewClient();

            // Assert
            client.Should().NotBeNull();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="HttpClientService"/> for use in tests
        /// </summary>
        private void CreateSut()
        {
            this._sut = new HttpClientService();
        }
    }
}
