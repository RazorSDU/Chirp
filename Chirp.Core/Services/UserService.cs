using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Chirp.Core.Domain.Interfaces.Repositories;

namespace Chirp.Core.Services;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersFromDatabaseAsync();
        var userDtos = new List<UserDto>();
        foreach (User user in users)
        {
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Posts = user.Posts,
            };
            userDtos.Add(userDto);
        }
        return userDtos;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserFromDatabaseByIdAsync(userId);
        var userDto = new UserDto()
        {
            Id = user.Id,
            Username = user.Username
        };
        
        return userDto;
    }


    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserFromDatabaseByUsernameAsync(username);
        if (user == null)
        {
            return null;
        }
        var userDto = new UserDto()
        {
            Id = user.Id,
            Username = user.Username
        };
        return userDto;
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
    {
        User newUser = new User()
        {
            Id = Guid.NewGuid(),
            Username = createUserDto.Username,
            PasswordHash = createUserDto.Password,
        };
        await _userRepository.SaveUserToDatabaseAsync(newUser);
        UserDto newUserDto = new UserDto()
        {
            Id = newUser.Id,
            Username = newUser.Username
        };
        return newUserDto;
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var updatedUser = await _userRepository.GetUserFromDatabaseByIdAsync(userId);
        updatedUser.Username = updateUserDto.Username;
        updatedUser.PasswordHash = updateUserDto.Password;
        await _userRepository.SaveUpdatedUserToDatabaseAsync(updatedUser);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteUserFromDatabaseAsync(userId);
    }

    public async Task<User?> GetUserForAuthAsync(string userName)
    {
        return await _userRepository.GetUserFromDatabaseByUsernameAsync(userName);
    }

}