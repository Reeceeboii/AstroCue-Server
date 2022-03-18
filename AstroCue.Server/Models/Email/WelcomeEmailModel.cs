namespace AstroCue.Server.Models.Email
{
    /// <summary>
    /// Model class representing the Handlebars data sent to MailGun
    /// as part of the welcome email, sent to new users when they first sign up
    /// </summary>
    public class WelcomeEmailModel
    {
        /// <summary>
        /// Gets or sets the new user's first name
        /// </summary>
        public string FirstName { get; set; }
    }
}
