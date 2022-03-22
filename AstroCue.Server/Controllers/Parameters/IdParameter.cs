namespace AstroCue.Server.Controllers.Parameters
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Query parameter binding for any endpoint that requires an ID
    /// </summary>
    public class IdParameter
    {
        /// <summary>
        /// The ID of the item
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
