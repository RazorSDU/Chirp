using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Chirp.Core.Services;
using Chirp.Tests.Backend.Core.Mocks;

namespace Chirp.Core.Tests
{
    public class UserServiceTests
    {

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnUsers()
        {
            // Arrange
            MockUserRepository userRepository = new MockUserRepository();
            UserService userService = new UserService(userRepository);

            var users = new List<User> 
            { 
                new User { Id = Guid.NewGuid(),Username = "Test User1" },
                new User { Id = Guid.NewGuid(),Username = "Test User2" },
                new User { Id = Guid.NewGuid(),Username = "Test User3" },

            };

            foreach (User user in users)
            {
                await userRepository.SaveUserToDatabaseAsync(user);
            }

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.IsType<List<UserDto>>(result);
            Assert.Equal(users.Count, result.Count());
            Assert.All(result, dto => Assert.Contains(users, u => u.Username == dto.Username));

        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateAndReturnUser()
        {
            // Arrange
            var userRepository = new MockUserRepository();
            var userService = new UserService(userRepository);
            var createDto = new CreateUserDto { Username = "New User", Password = "<PASSWORD>"};

            // Act
            var result = await userService.CreateUserAsync(createDto);

            // Assert
            Assert.Equal("New User", result.Username);
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal(createDto.Username, result.Username);

            var allUsers = await userRepository.GetAllUsersFromDatabaseAsync();
            var savedUser = Assert.Single(allUsers);
            Assert.Equal(createDto.Username, savedUser.Username);
            Assert.NotEqual(Guid.Empty, savedUser.Id);


        }
    }
}