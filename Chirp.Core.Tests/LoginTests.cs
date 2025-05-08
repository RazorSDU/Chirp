using Chirp.API.Authentication;
using Chirp.API.DTOs.Auth;
using Chirp.Core.DTOs;
using Chirp.Tests.Backend.Mocks;

namespace Chirp.Core.Tests;

public class LoginTests
{
    [Fact]
    public async Task LoginAsync_ShouldReturnToken_ForExistingUser()
    {
        // Arrange
        var mockUserService = new MockUserService();
        var mockPasswordHasher = new MockPasswordHasher();
        var mockConfig = new MockConfiguration();
        var authService = new AuthService(mockUserService, mockConfig, mockPasswordHasher);

        await authService.RegisterAsync(new CreateUserDto { Username = "newuser", Password = "pass123" });


        var loginDto = new LoginDto { Username = "newuser", Password = "pass123" };

        // Act
        var response = await authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("newuser", response.Username);
        Assert.False(string.IsNullOrWhiteSpace(response.Token));
        Assert.True(response.Expires > DateTime.UtcNow);

    }
}