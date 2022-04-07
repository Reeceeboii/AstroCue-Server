namespace AstroCue.Server.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Data;
    using Entities;
    using Interfaces;
    using Utilities;

    /// <summary>
    /// Service to handle authentication operations
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/>
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Instance of <see cref="IEmailService"/>
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initialises a new instance of the <see cref="AuthService"/> class
        /// </summary>
        /// <param name="context">Instance of <see cref="ApplicationDbContext"/></param>
        /// <param name="emailService">Instance of <see cref="IEmailService"/></param>
        public AuthService(ApplicationDbContext context, IEmailService emailService)
        {
            this._context = context;
            this._emailService = emailService;
        }

        /// <summary>
        /// Register a new AstroCue user
        /// </summary>
        /// <param name="newUser">Instance of <see cref="AstroCueUser"/></param>
        /// <param name="password">A plaintext password</param>
        /// <returns>Instance of <see cref="AstroCueUser"/></returns>
        /// <exception cref="ArgumentException">Isufficient password, or user registered with
        /// provided email already exists in database</exception>
        public AstroCueUser RegisterAstroCueUser(AstroCueUser newUser, string password)
        {
            // check that password is present
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password needs to be present");
            }

            // carry out capitalisation and trimming etc...
            newUser.EmailAddress = StringUtilities.TrimToLowerAll(newUser.EmailAddress);
            newUser.FirstName = StringUtilities.TrimToUpperFirstChar(newUser.FirstName, true);
            newUser.LastName = StringUtilities.TrimToUpperFirstChar(newUser.LastName, true);

            // check that email address isn't already registered with AstroCue
            if (this._context.AstroCueUsers.Any(existingUser => existingUser.EmailAddress == newUser.EmailAddress))
            {
                throw new ArgumentException($"An account with email {newUser.EmailAddress} already exists");
            }

            // salt and hash incoming password
            using HMACSHA512 hmacSha512 = new();

            newUser.PasswordSalt = hmacSha512.Key;
            newUser.PasswordHash = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(password));

            // add new user to database context
            this._context.AstroCueUsers.Add(newUser);
            this._context.SaveChanges();

            // send welcome email
            this._emailService.SendWelcomeEmail(newUser);

            return newUser;
        }

        /// <summary>
        /// Authenticate a user against the database
        /// </summary>
        /// <param name="emailAddress">An email address given in the authentication attempt</param>
        /// <param name="password">A password provided in the authentication attempt</param>
        /// <returns>An instance of <see cref="AstroCueUser"/></returns>
        /// <exception cref="ArgumentException">If the password is not present</exception>
        public AstroCueUser AuthenticateAstroCueUser(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            emailAddress = StringUtilities.TrimToLowerAll(emailAddress);

            AstroCueUser astroCueUser =
                this._context.AstroCueUsers.SingleOrDefault(user => user.EmailAddress == emailAddress);

            if (astroCueUser == null)
            {
                return null;
            }

            // At this point, a valid user matching the given email address has been found,
            // so we can attempt an authentication

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password needs to be present");
            }

            using HMACSHA512 hmacSha512 = new(astroCueUser.PasswordSalt);
            byte[] computed = hmacSha512.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computed.Length; ++i)
            {
                if (computed[i] != astroCueUser.PasswordHash[i])
                {
                    return null;
                }
            }

            return astroCueUser;
        }
    }
}
