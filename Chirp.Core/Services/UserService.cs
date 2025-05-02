using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
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

    public async Task<IEnumerable<UserDto>> GetUsersByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        User newUser = new User()
        {
            Id = Guid.NewGuid(),
            Username = createUserDto.Username,
            PasswordHash = createUserDto.Password,
        };
        await _userRepository.SaveUserToDatabaseAsync(newUser);
        UserDto newUserDto = new UserDto();
        {
            Guid Id = newUser.Id;
            string Username = newUser.Username;
        }
        return newUserDto;
    }
}