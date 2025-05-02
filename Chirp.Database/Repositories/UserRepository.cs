using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Chirp.Core.Domain.Interfaces.Repositories;

namespace Chirp.Database.Repositories;

public class UserRepository : IUserRepository

{
    private readonly ChirpContext _context;

    public UserRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task SaveUserToDatabaseAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> SaveUpdatedUserToDatabaseAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserFromDatabaseAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserFromDatabaseByIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllUsersFromDatabaseAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
