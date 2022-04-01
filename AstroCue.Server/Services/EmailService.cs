namespace AstroCue.Server.Services
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Interfaces;
    using Entities;
    using Interfaces;
    using Models.Email;
    using Models.Email.Reports;
    using Newtonsoft.Json;
    using RestSharp;

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
        /// Instance of <see cref="IHttpClientService"/>
        /// </summary>
        private readonly IHttpClientService _httpClientService;

        /// <summary>
        /// Instance of <see cref="RestClient"/>
        /// </summary>
        private readonly RestClient _client;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Name of the MailGun template to access when sending the welcome emails to new users
        /// </summary>
        private const string WelcomeEmailTemplate = "welcome-template";

        /// <summary>
        /// Name of the MainGun template to access when sending a report email to a user
        /// </summary>
        private const string ReportEmailTemplate = "report-template";

        /// <summary>
        /// Initialises a new instance of the <see cref="EmailService"/> class
        /// </summary>
        /// <param name="environmentManager">Instance of <see cref="IEnvironmentManager"/></param>
        /// <param name="httpClientService">Instance of <see cref="IHttpClientService"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        public EmailService(
            IEnvironmentManager environmentManager,
            IHttpClientService httpClientService,
            IMapper mapper)
        {
            this._environmentManager = environmentManager;
            this._httpClientService = httpClientService;
            this._mapper = mapper;

            this._client = this._httpClientService.NewClientBasicAuth(
                this._environmentManager.MailGunApiKey,
                this._environmentManager.BaseMailGunMessagesUrl);
        }

        /// <summary>
        /// Send an email to a newly signed up user welcoming them to AstroCue
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        /// <returns><see cref="RestResponse"/></returns>
        public async Task<RestResponse> SendWelcomeEmail(AstroCueUser user)
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

            return await this._client.ExecuteAsync(req);
        }

        /// <summary>
        /// Send a report email to a user
        /// </summary>
        /// <param name="user">An instance of <see cref="AstroCueUser"/></param>
        /// <param name="reportList">A list of <see cref="LocationReport"/> instances</param>
        /// <param name="staticMaps">Static map image mappings to their filenames</param>
        /// <returns><see cref="RestResponse"/></returns>
        public async Task<RestResponse> SendReportEmail(
            AstroCueUser user, 
            List<LocationReport> reportList,
            Dictionary<string, byte[]> staticMaps)
        {
            /*
             * For some really REALLY odd reason beyond my understanding, the MailGun API does not accept
             * a Handlebars payload whose outermost element is an array (i.e. you cannot just pass it a serialised List<T> via the
             * "h:X-Mailgun-Variables" parameter).
             *
             * Because of this absolutely absurd restriction, I am forced to use a dynamic object and apply numbered properties to it
             * at runtime based on the indexes from the original list that any sane person would just send as is.
             *
             * I could probably implement a custom serialiser to get around this, but I don't have enough time.
             *
             * So... enter this monstrosity (or maybe it's not that bad, I don't know. It just feels wrong):
             */
            dynamic templateObject = new ExpandoObject();
            for (int i = 0; i < reportList.Count; i++)
            {
                IDictionary<string, object> props = templateObject as IDictionary<string, object>;
                props.Add($"{i}", reportList[i]);
            }

            // it works, but at what cost??
            string templateData = JsonConvert.SerializeObject(templateObject);

            // calculate the total number of reports generated
            int totalReports = reportList.Sum(r => r.Objects.Sum(x => x.Count));

            RestRequest req = new();
            req.AddParameter("domain", "astrocue.co.uk", ParameterType.UrlSegment);
            req.Resource = "{domain}/messages";
            req.AddParameter("from", "AstroCue <no-reply@astrocue.co.uk>");
            req.AddParameter("to", user.EmailAddress);
            req.AddParameter("subject", totalReports > 1 ? $"Your {totalReports} reports!" : "Your 1 report!");
            req.AddParameter("template", ReportEmailTemplate);
            req.AddParameter("h:X-Mailgun-Variables", templateData);

            // add each static map file as an attachment
            foreach ((string key, byte[] value) in staticMaps)
            {
                req.AddFile("inline", value, key);
            }

            req.Method = Method.Post;

            RestResponse res = this._client.ExecuteAsync(req).Result;

            return await this._client.ExecuteAsync(req);
        }
    }
}
