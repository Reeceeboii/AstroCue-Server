namespace AstroCue.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using RestSharp;
    using RestSharp.Authenticators;
    using RichardSzalay.MockHttp;
    using Server.Data.Interfaces;
    using Server.Entities;
    using Server.Models.Email;
    using Server.Services;
    using Server.Services.Interfaces;

    /// <summary>
    /// Class for testing <see cref="EmailService"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EmailServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private EmailService _sut;

        /// <summary>
        /// Mock <see cref="IEnvironmentManager"/> instance used by tests
        /// </summary>
        private readonly Mock<IEnvironmentManager> _mockEnvironmentManager;

        /// <summary>
        /// Mock <see cref="IHttpClientService"/> instance used by the tests
        /// </summary>
        private readonly Mock<IHttpClientService> _mockHttpClientService;

        /// <summary>
        /// Mock <see cref="IMapper"/> instance used by the tests
        /// </summary>
        private readonly Mock<IMapper> _mockMapper;

        /// <summary>
        /// Mock HttpMessageHandler used by the tests
        /// </summary>
        private readonly MockHttpMessageHandler _mockHttpMessageHandler;

        /// <summary>
        /// Fake MailGun API key used by the tests
        /// </summary>
        private const string FakeMailGunApiKey = "B76C88D4-AEC5-46B2-9FBA-CFEB505173B0";

        /// <summary>
        /// Fake MailGun messages API address used by the tests
        /// </summary>
        private const string FakeBaseApiUrl = "https://localhost";

        /// <summary>
        /// The URL reached by the HTTP client requests during the tests, used for request matching
        /// with <see cref="MockHttpMessageHandler"/>
        /// </summary>
        private readonly string _messagesResourceUrl = $"{FakeBaseApiUrl}/astrocue.co.uk/messages";

        /// <summary>
        /// Initialises a new instance of the <see cref="EmailServiceTests"/> class
        /// </summary>
        public EmailServiceTests()
        {
            this._mockEnvironmentManager = new Mock<IEnvironmentManager>();
            this._mockHttpClientService = new Mock<IHttpClientService>();
            this._mockMapper = new Mock<IMapper>();

            this._mockHttpMessageHandler = new MockHttpMessageHandler();

            RestClientOptions opts = new()
            {
                ConfigureMessageHandler = _ => this._mockHttpMessageHandler,
                BaseUrl = new Uri(FakeBaseApiUrl)
            };

            HttpClient client = this._mockHttpMessageHandler.ToHttpClient();
            client.BaseAddress = new Uri(FakeBaseApiUrl);

            this._mockHttpClientService
                .Setup(i => i.NewClientBasicAuth(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new RestClient(client, opts)
                {
                    Authenticator = new HttpBasicAuthenticator("api", FakeMailGunApiKey)
                });

            this._mockEnvironmentManager
                .Setup(i => i.BaseMailGunMessagesUrl)
                .Returns(FakeBaseApiUrl);

            this._mockEnvironmentManager
                .Setup(i => i.MailGunApiKey)
                .Returns(FakeMailGunApiKey);

            this._mockMapper
                .Setup(i => i.Map<WelcomeEmailModel>(It.IsAny<AstroCueUser>()))
                .Returns(new WelcomeEmailModel()
                {
                    FirstName = "Joe"
                });
        }

        /// <summary>
        /// Tests that <see cref="EmailService"/> can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialisedEmailServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a welcome email can be sent from <see cref="EmailService"/>
        /// </summary>
        [TestMethod]
        public void CanSendWelcomeEmailTest()
        {
            // Arrange
            AstroCueUser user = new()
            {
                FirstName = "Joe",
                EmailAddress = "example@test.com"
            };

            // set up a fake endpoint for the mock client to hit
            MockedRequest mockedRequest = this._mockHttpMessageHandler
                .When(this._messagesResourceUrl)
                .WithFormData(new Dictionary<string, string>
                {
                    { "to", user.EmailAddress },
                    { "from", "AstroCue <no-reply@astrocue.co.uk>" },
                    { "template", "welcome-template" }
                })
                .Respond(HttpStatusCode.OK);

            // Act
            this.CreateSut();
            RestResponse res = this._sut.SendWelcomeEmail(user).Result;

            // Assert
            this._mockHttpMessageHandler.GetMatchCount(mockedRequest).Should().Be(1);
            res.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Initialises a new <see cref="EmailService"/> instance for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new EmailService(
                this._mockEnvironmentManager.Object,
                this._mockHttpClientService.Object,
                this._mockMapper.Object);
        }
    }
}
