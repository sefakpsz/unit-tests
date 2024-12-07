using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;

namespace RunGroopWebApp.Tests.Repository
{
    public class ClubRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();

            if (!await databaseContext.Clubs.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                        new Club
                        {
                            Title = "Running Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        }
                    );

                    await databaseContext.SaveChangesAsync();
                }
            }

            return databaseContext;
        }

        [Fact]
        public async Task ClubRepository_Add_ReturnsBool()
        {
            //Arrange
            var club = new Club()
            {
                Title = "Running Club 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg",
                Description = "This is the description of the first cinema",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };

            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.Add(club);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }
    }
}