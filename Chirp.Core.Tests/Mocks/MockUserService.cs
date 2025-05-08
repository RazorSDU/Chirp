using Chirp.API.Authentication;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Chirp.Tests.Backend.Mocks;


    public class MockUserService : IUserService
    {
        private readonly Dictionary<string, User> _users = new();
        public string? CreatedUserPassword;

        public Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            if (_users.TryGetValue(username, out var user))
            {
                return Task.FromResult<UserDto?>(new UserDto { Id = user.Id, Username = user.Username });
            }

            return Task.FromResult<UserDto?>(null);
        }



        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            CreatedUserPassword = createUserDto.Password;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = createUserDto.Username,
                PasswordHash = createUserDto.Password
            };
            _users[user.Username] = user;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };

        }

        public Task<User?> GetUserForAuthAsync(string username)
        {
            _users.TryGetValue(username, out var user);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return Task.FromResult<IEnumerable<UserDto>>(_users.Values.Select(u => new UserDto { Id = u.Id, Username = u.Username }));
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = _users.Values.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }
            var userDto = new UserDto { Id = user.Id, Username = user.Username };
            return userDto;

        }
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
                    "Jwt:Key" => "supersecretkey12345678_hide_it_away!",
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