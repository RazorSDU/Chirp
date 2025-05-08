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

    public async Task DeleteUserFromDatabaseAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

    }

    public async Task<User> GetUserFromDatabaseByIdAsync(Guid userId)
    {
        try
        {
            return await _context.Users.FindAsync(userId);
        }
        catch
        {
            throw new Exception("User not found");
        }

    }

    public async Task<User> GetUserFromDatabaseByUsernameAsync(string username)
    {
        return await _context.Users
            .SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllUsersFromDatabaseAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public Task<bool> ExistsAsync(Guid id)
    => _context.Users.AnyAsync(u => u.Id == id);

}
