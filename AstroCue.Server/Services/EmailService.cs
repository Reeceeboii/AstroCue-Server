﻿namespace AstroCue.Server.Services
{
    using System;
    using AutoMapper;
    using Data.Interfaces;
    using Entities;
    using Interfaces;
    using Models.Email;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Service class for handling email communications
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/>
        /// </summary>
        private readonly IEnvironmentManager _environmentManager;

        /// <summary>
        /// Instance of <see cref="RestClient"/>
        /// </summary>
        private readonly RestClient _client;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Base URL of the MapBox messages API
        /// </summary>
        private const string BaseMessagesUrl = "https://api.eu.mailgun.net/v3";

        /// <summary>
        /// Name of the MailGun template to access when sending the welcome emails to new users
        /// </summary>
        private const string WelcomeEmailTemplate = "welcome-template";

        /// <summary>
        /// Initialises a new instance of the <see cref="EmailService"/> class
        /// </summary>
        /// <param name="environmentManager">Instance of <see cref="IEnvironmentManager"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public EmailService(IEnvironmentManager environmentManager, IMapper mapper)
        {
            this._environmentManager = environmentManager;
            this._mapper = mapper;
            RestClientOptions opts = new(new Uri(BaseMessagesUrl));

            this._client = new RestClient(opts)
            {
                Authenticator = new HttpBasicAuthenticator("api", this._environmentManager.MailGunApiKey)
            };
        }

        /// <summary>
        /// Send an email to a newly signed up user welcoming them to AstroCue
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        public async void SendWelcomeEmail(AstroCueUser user)
        {
            string templateData = JsonConvert.SerializeObject(this._mapper.Map<WelcomeEmailModel>(user));

            RestRequest req = new();
            req.AddParameter("domain", "astrocue.co.uk", ParameterType.UrlSegment);
            req.Resource = "{domain}/messages";
            req.AddParameter("from", "AstroCue <no-reply@astrocue.co.uk>");
            req.AddParameter("to", user.EmailAddress);
            req.AddParameter("subject", $"Welcome, {user.FirstName}!");
            req.AddParameter("template", WelcomeEmailTemplate);
            req.AddParameter("h:X-Mailgun-Variables", templateData);
            req.Method = Method.Post;

            await this._client.ExecuteAsync(req);
        }
    }
}
