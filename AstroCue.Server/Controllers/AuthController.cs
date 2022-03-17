namespace AstroCue.Server.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using AutoMapper;
    using Data.Interfaces;
    using Entities;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Models.API.Inbound;
    using Models.API.Outbound;
    using Services.Interfaces;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Controller to handle user authentication operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Instance of <see cref="IAuthenticationService"/>
        /// </summary>
        private readonly IAuthService _authService;

        /// <summary>
        /// Instance of <see cref="IAstroCueUserService"/>
        /// </summary>
        private readonly IAstroCueUserService _astroCueUserServiceService;

        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/>
        /// </summary>
        private readonly IEnvironmentManager _environmentManager;

        /// <summary>
        /// Instance of <see cref="IMapper"/>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialises a new instance of the <see cref="AuthController"/> class
        /// </summary>
        /// <param name="authService">Instance of <see cref="IAuthService"/></param>
        /// <param name="astroCueUserServiceService">Instance of <see cref="IAstroCueUserService"/></param>
        /// <param name="mapper">Instance of <see cref="IMapper"/></param>
        /// <param name="environmentManager">Instance of <see cref="IEnvironmentManager"/></param>
        public AuthController(
            IAuthService authService,
            IAstroCueUserService astroCueUserServiceService,
            IMapper mapper,
            IEnvironmentManager environmentManager)
        {
            this._authService = authService;
            this._astroCueUserServiceService = astroCueUserServiceService;
            this._mapper = mapper;
            this._environmentManager = environmentManager;
        }

        /// <summary>
        /// Registers a new user with AstroCue
        /// </summary>
        /// <param name="inboundRegModel">An instance of <see cref="InboundRegModel"/></param>
        /// <returns>Status code representing the outcome of the request, or an error</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [SwaggerOperation(
            Summary = "User registration",
            Description = "Register a new user with an email, name and password")]
        [SwaggerResponse(StatusCodes.Status200OK, "New user registered successfully")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The registration failed", typeof(OutboundErrorModel))]
        public IActionResult Register([FromBody] InboundRegModel inboundRegModel)
        {
            AstroCueUser astroCueUser = this._mapper.Map<AstroCueUser>(inboundRegModel);

            try
            {
                this._authService.RegisterAstroCueUser(astroCueUser, inboundRegModel.Password);
                return this.Ok();
            }
            catch (Exception exc)
            {
                return this.BadRequest(new OutboundErrorModel()
                {
                    Message = exc.Message
                });
            }
        }

        /// <summary>
        /// Authenticates a user against the database
        /// </summary>
        /// <param name="inboundAuthModel">An instance of <see cref="InboundAuthModel"/></param>
        /// <returns>Either an <see cref="OutboundSuccessfulAuthModel"/>
        /// or an <see cref="OutboundErrorModel"/></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        [SwaggerOperation(
            Summary = "User authentication",
            Description = "Authenticates a user using an email address and a password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Authentication was successful", typeof(OutboundAuthSuccessModel))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Authentication was not successful", typeof(OutboundErrorModel))]
        public IActionResult Login([FromBody] InboundAuthModel inboundAuthModel)
        {
            AstroCueUser astroCueUser = this._authService.AuthenticateAstroCueUser(
                inboundAuthModel.EmailAddress,
                inboundAuthModel.Password);

            if (astroCueUser == null)
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, new OutboundErrorModel()
                {
                    Message = "Your email or password is incorrect"
                });
            }

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, astroCueUser.Id.ToString())
                }),
                Expires = DateTime.Now.AddMonths(6),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._environmentManager.JwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new();

            OutboundAuthSuccessModel authModel = this._mapper.Map<OutboundAuthSuccessModel>(astroCueUser);
            authModel.Token = handler.WriteToken(handler.CreateToken(descriptor));

            return this.Ok(authModel);
        }
    }
}
