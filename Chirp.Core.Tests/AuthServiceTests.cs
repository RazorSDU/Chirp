/*using Chirp.API.Authentication;
using Chirp.API.DTOs.Auth;
using Chirp.Core.Domain.Entities;
using Chirp.Tests.Backend.Core.Mocks;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Tests;

public class AuthServiceTests
{
    private MockUserService mockUserService = new();
    private MockPasswordHasher mockPasswordHasher = new();
    private MockConfiguration mockConfiguration = new();
    private AuthService authService;

    public AuthServiceTests()
    {
        authService = new AuthService(mockUserService, mockConfiguration, mockPasswordHasher);
    }

    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenValidCredentials()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "john", PasswordHash = "hashed_johnspass" };
        mockUserService.LoginUser = user;
        mockPasswordHasher.ResultToReturn = PasswordVerificationResult.Success;

        var dto = new LoginDto { Username = "john", Password = "johnspass" };
        var response = await authService.LoginAsync(dto);

        Assert.NotNull(response.Token);
        Assert.Equal("john", response.Username);
    }

    [Fact]
    public async Task LoginAsync_ThrowsException_WhenUserNotFound()
    {
        mockUserService.LoginUser = null;

        var dto = new LoginDto { Username = "missing", Password = "irrelevant" };

        await Assert.ThrowsAsync<Exception>(() => authService.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ThrowsException_WhenPasswordWrong()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "john", PasswordHash = "hashed_johnspass" };
        mockUserService.LoginUser = user;
        mockPasswordHasher.ResultToReturn = PasswordVerificationResult.Failed;

        var dto = new LoginDto { Username = "john", Password = "wrongpass" };

        await Assert.ThrowsAsync<Exception>(() => authService.LoginAsync(dto));
    }

}
*/