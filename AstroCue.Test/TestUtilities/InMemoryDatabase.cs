namespace AstroCue.Test.TestUtilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using Server.Data;

    /// <summary>
    /// In memory DB related test util methods
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class InMemoryDatabase
    {
        /// <summary>
        /// Create a new in memory <see cref="ApplicationDbContext"/>
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext NewInMemoryDbContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // if this results in a database clash then 🤷
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
