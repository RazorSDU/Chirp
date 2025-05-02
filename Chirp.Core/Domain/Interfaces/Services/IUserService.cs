using Chirp.Core.DTOs;

namespace Chirp.Core.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IEnumerable<UserDto>> GetUsersByIdAsync(int userId);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
}