namespace AstroCue.Server.Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Entity framework core database context class, inherits from <see cref="DbContext"/>
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationDbContext"/> class
        /// </summary>
        /// <param name="contextOptions">Instance of <see cref="DbContextOptions"/></param>
        public ApplicationDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        /// <summary>
        /// Override of <see cref="DbContext.OnModelCreating"/> to further configure models and relationships
        /// </summary>
        /// <param name="modelBuilder">Instance of <see cref="ModelBuilder"/></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Observation>()
                .HasOne(l => l.ObservationLocation)
                .WithMany(a => a.Observations)
                .HasForeignKey(l => l.ObservationLocationId);

            modelBuilder.Entity<Observation>()
                .HasOne(l => l.AstronomicalObject)
                .WithMany(a => a.Observations)
                .HasForeignKey(l => l.AstronomicalObjectId);
        }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="AstronomicalObject"/> entities
        /// </summary>
        public DbSet<AstronomicalObject> AstronomicalObjects { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="HipObject"/> entities
        /// that inherit from <see cref="AstronomicalObject"/>
        /// </summary>
        public DbSet<HipObject> HipObjects { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="NgcObject"/> entities
        /// that inherit from <see cref="AstronomicalObjects"/>
        /// </summary>
        public DbSet<NgcObject> NgcObjects { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="AstroCueUser"/> entities
        /// </summary>
        public DbSet<AstroCueUser> AstroCueUsers { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="ObservationLocation"/> entities
        /// </summary>
        public DbSet<ObservationLocation> ObservationLocations { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="Observation"/> entities
        /// </summary>
        public DbSet<Observation> Observations { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="Report"/> entities
        /// </summary>
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="DbSet{TEntity}"/> of <see cref="ObservationLog"/> entities
        /// </summary>
        public DbSet<ObservationLog> ObservationLogs { get; set; }
    }
}
