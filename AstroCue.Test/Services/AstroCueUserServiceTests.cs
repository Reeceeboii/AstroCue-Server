namespace AstroCue.Test.Services
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Server.Data;
    using Server.Entities;
    using Server.Services;

    /// <summary>
    /// Tests targeting <see cref="AstroCueUserService"/>
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AstroCueUserServiceTests
    {
        /// <summary>
        /// System under test
        /// </summary>
        private AstroCueUserService _sut;

        /// <summary>
        /// Instance of <see cref="ApplicationDbContext"/> used by tests
        /// </summary>
        private readonly ApplicationDbContext _inMemoryDbContext;

        /// <summary>
        /// <see cref="AstroCueUser"/> used by tests
        /// </summary>
        private readonly AstroCueUser _testUser;

        /// <summary>
        /// Initialises a new instance of the <see cref="AstroCueUserServiceTests"/> class
        /// </summary>
        public AstroCueUserServiceTests()
        {
            this._inMemoryDbContext = TestUtilities.InMemoryDatabase.NewInMemoryDbContext();

            this._testUser = new AstroCueUser()
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                EmailAddress = "example@test.com",
                PasswordHash = Encoding.ASCII.GetBytes("somepassword"),
                PasswordSalt = Encoding.ASCII.GetBytes("somehash")
            };

            this.SeedTestDatabase();
        }

        /// <summary>
        /// Seed the test database with sample data
        /// </summary>
        private void SeedTestDatabase()
        {
            this._inMemoryDbContext.AstroCueUsers.Add(this._testUser);
            this._inMemoryDbContext.SaveChanges();
        }

        /// <summary>
        /// Tests that the <see cref="AstroCueUserService"/> class can be initialised
        /// </summary>
        [TestMethod]
        public void CanInitialisedAstroCueUserServiceTest()
        {
            // Arrange
            // Act
            this.CreateSut();

            // Assert
            this._sut.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that a user can be retrieved by their ID
        /// </summary>
        [TestMethod]
        public void RetrieveByIdTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser user = this._sut.RetrieveById(this._testUser.Id);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(this._testUser.Id);
        }

        /// <summary>
        /// Tests that retrieving a non-existent user returns a null value
        /// </summary>
        [TestMethod]
        public void RetrieveMissingIdTest()
        {
            // Arrange
            // Act
            this.CreateSut();
            AstroCueUser user = this._sut.RetrieveById(10000);

            // Assert
            user.Should().BeNull();
        }

        /// <summary>
        /// Initialise a new instance of <see cref="AstroCueUserService"/> for testing
        /// </summary>
        private void CreateSut()
        {
            this._sut = new AstroCueUserService(this._inMemoryDbContext);
        }
    }
}
