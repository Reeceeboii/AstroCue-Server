namespace AstroCue.Server.Services.Interfaces
{
    using System;
    using Entities;

    /// <summary>
    /// Interface for <see cref="AuthService"/>
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Register a new AstroCue user
        /// </summary>
        /// <param name="newUser">Instance of <see cref="AstroCueUser"/></param>
        /// <param name="password">A plaintext password</param>
        /// <returns>Instance of <see cref="AstroCueUser"/></returns>
        /// <exception cref="ArgumentException">Isufficient password, or user registered with
        /// provided email already exists in database</exception>
        AstroCueUser RegisterAstroCueUser(AstroCueUser newUser, string password);

        /// <summary>
        /// Authenticate a user against the database
        /// </summary>
        /// <param name="emailAddress">An email address given in the authentication attempt</param>
        /// <param name="password">A password provided in the authentication attempt</param>
        /// <returns>An instance of <see cref="AstroCueUser"/></returns>
        /// <exception cref="ArgumentException">If the password is not present</exception>
        AstroCueUser AuthenticateAstroCueUser(string emailAddress, string password);
    }
}