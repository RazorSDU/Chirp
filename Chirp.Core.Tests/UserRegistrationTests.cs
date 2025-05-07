using Chirp.API.Authentication;
using Chirp.Core.DTOs;
using Chirp.Tests.Backend.Mocks;

namespace Chirp.Core.Tests;


public class UserRegistrationTests
{
        [Fact]
        public async Task RegisterAsync_CreatesAndReturnsToken_ForNewUser()
        {
            // Arrange
            var mockUserService = new MockUserService();
            var mockPasswordHasher = new MockPasswordHasher();
            var mockConfig = new MockConfiguration();
            var authService = new AuthService(mockUserService, mockConfig, mockPasswordHasher);


            var dto = new CreateUserDto { Username = "newuser", Password = "pass123" };

            // Act
            var response = await authService.RegisterAsync(dto);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("newuser", response.Username);
            Assert.False(string.IsNullOrWhiteSpace(response.Token));
            Assert.True(response.Expires > DateTime.UtcNow);
            Assert.Equal("hashed_pass123", mockUserService.CreatedUserPassword);
        }
}