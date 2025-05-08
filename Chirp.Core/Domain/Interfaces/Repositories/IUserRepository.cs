using Chirp.Core.Domain.Entities;

namespace Chirp.Core.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task SaveUserToDatabaseAsync(User user);
    Task<User> SaveUpdatedUserToDatabaseAsync(User user);
    Task DeleteUserFromDatabaseAsync(Guid userId);
    Task<User> GetUserFromDatabaseByIdAsync(Guid userId);
    Task<User> GetUserFromDatabaseByUsernameAsync(string username);
    Task<IEnumerable<User>> GetAllUsersFromDatabaseAsync();
    Task<bool> ExistsAsync(Guid id);

}