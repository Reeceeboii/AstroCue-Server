namespace AstroCue.Server.Data
{
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using Entities;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Parsers;

    /// <summary>
    /// Class for seeding the database with its required data
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Seed the database with the astronomical catalogues
        /// </summary>
        /// <param name="applicationBuilder">Instance of <see cref="IApplicationBuilder"/></param>
        public static void SeedAstronomicalCatalogues(IApplicationBuilder applicationBuilder)
        {
            using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();
            ApplicationDbContext ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            IFileSystem fileSystem = scope.ServiceProvider.GetRequiredService<IFileSystem>();

            ctx.ChangeTracker.AutoDetectChangesEnabled = false;

            // if no astronomical objects are present in the relevant table
            if (!ctx.AstronomicalObjects.Any())
            {
                NgcParser ngcParser = new(fileSystem);
                HipParser hipParser = new(fileSystem);

                List<AstronomicalObject> ngcObjects = ngcParser.ParseCatalogue();
                List<AstronomicalObject> hipObjects = hipParser.ParseCatalogue();

                ctx.NgcObjects.AddRange(ngcObjects.Cast<NgcObject>().ToList());
                ctx.HipObjects.AddRange(hipObjects.Cast<HipObject>().ToList());
                ctx.SaveChanges();
            }

            ctx.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }
}