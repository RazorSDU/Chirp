/*using Chirp.API.Authentication;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Chirp.Tests.Backend.Core.Mocks;


    public class MockUserService : IUserService
    {
        public User? ExistingUser;
        public User? LoginUser;
        public string? CreatedUserPassword;

        public Task<UserDto> GetUserByUsernameAsync(string username)
            => Task.FromResult(ExistingUser);

        public Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            CreatedUserPassword = dto.Password;
            return Task.CompletedTask;
        }

        public Task<User?> GetUserForAuthAsync(string username)
            => Task.FromResult(LoginUser);
        public Task<IEnumerable<UserDto>> GetAllUsersAsync() => throw new NotImplementedException();
        public Task<UserDto?> GetUserByIdAsync(Guid id) => throw new NotImplementedException();
        public Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto) => throw new NotImplementedException();
        public Task DeleteUserAsync(Guid userId) => throw new NotImplementedException();
    }

    public class MockPasswordHasher : IPasswordHasher<User>
    {
        public PasswordVerificationResult ResultToReturn = PasswordVerificationResult.Success;

        public string HashPassword(User user, string password)
            => $"hashed_{password}";

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
            => ResultToReturn;
    }

    public class MockConfiguration : IConfiguration
    {
        public string? this[string key]
        {
            get
            {
                return key switch
                {
                    "Jwt:Key" => "supersecretkey12345678",
                    "Jwt:Issuer" => "testissuer",
                    "Jwt:Audience" => "testaudience",
                    _ => null
                };
            }
            set => throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }
    }

*/