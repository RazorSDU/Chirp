using Chirp.Core.DTOs;
using Chirp.Core.Domain.Entities;

namespace Chirp.Core.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
    Task DeleteUserAsync(Guid userId);
    Task<User> GetUserForAuthAsync(string username);
}