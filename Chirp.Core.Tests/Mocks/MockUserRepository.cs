using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;

namespace Chirp.Tests.Backend.Core.Mocks;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _usertable = new List<User>();
    public async Task SaveUserToDatabaseAsync(User user)
    {
        _usertable.Add(user);
    }

    public async Task<User> SaveUpdatedUserToDatabaseAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserFromDatabaseAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserFromDatabaseByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllUsersFromDatabaseAsync()
    {
        return _usertable;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}