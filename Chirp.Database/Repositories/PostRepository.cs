using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Database.Repositories
{
    public sealed class PostRepository : IPostRepository
    {
        private readonly ChirpContext _db;

        public PostRepository(ChirpContext db) => _db = db;

        public Task<Post?> GetByIdAsync(Guid id)
            => _db.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Post>> GetRootPageAsync(int page, int pageSize)
            => await _db.Posts
                .Where(p => p.ParentPostId == null)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<IEnumerable<Post>> GetRepliesPageAsync(Guid postId, int page, int pageSize)
            => await _db.Posts
                .Where(p => p.ParentPostId == postId)
                .OrderBy(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public async Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize)
        {
            return await _db.Posts
                .Where(p => p.User.Username == username)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task AddAsync(Post post)
            => await _db.Posts.AddAsync(post);

        public Task SaveChangesAsync()
            => _db.SaveChangesAsync();
    }
}

