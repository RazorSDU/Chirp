using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Database.Repositories
{
    public sealed class PostRepository : IPostRepository
    {
        private readonly ChirpContext _db;

        public PostRepository(ChirpContext db) => _db = db;

        /* ───────────────────────────────────────────────────
           1. GetAll – get the most recent posts in the feed            
        */

        public Task<Post?> GetByIdAsync(Guid id)
            => _db.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .FirstOrDefaultAsync(p => p.Id == id);

        /* ───────────────────────────────────────────────────
           2. GetById – get a post by its ID         
        */
        public async Task<IEnumerable<Post>> GetRootPageAsync(int page, int pageSize)
            => await _db.Posts
                .Where(p => p.ParentPostId == null)
                .OrderByDescending(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        /* ───────────────────────────────────────────────────
           3. GetByUser – get posts by a specific user          
        */
        public async Task<IEnumerable<Post>> GetRepliesPageAsync(Guid postId, int page, int pageSize)
            => await _db.Posts
                .Where(p => p.ParentPostId == postId)
                .OrderBy(p => p.CreatedAt)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        // Without pagination
        public async Task<IEnumerable<Post>> GetRepliesAsync(Guid parentPostId)
        {
            return await _db.Posts
                .Where(p => p.ParentPostId == parentPostId)
                .Include(p => p.User)
                .Include(p => p.Replies)
                .ToListAsync();
        }

        /* ───────────────────────────────────────────────────
           4. GetByUser – get posts by a specific user            
        */
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

        /* ───────────────────────────────────────────────────
           5. Create – Add a new post or comment to the feed        
        */
        public async Task AddAsync(Post post)
            => await _db.Posts.AddAsync(post);

        /* ───────────────────────────────────────────────────
           6. Update – edit an existing post        
        */
        public void Update(Post post)
            => _db.Posts.Update(post);

        /* ───────────────────────────────────────────────────
           7. Delete – remove a post from the feed        
        */
        public void Remove(Post post)
            => _db.Posts.Remove(post);

        /* ───────────────────────────────────────────────────
           8. Search – find posts by criteria      
        */
        public async Task<IEnumerable<Post>> SearchAsync(
            PostSearchCriteria criteria,
            int page,
            int pageSize)
        {
            var query = _db.Posts
                .Include(p => p.User)
                .Include(p => p.Replies)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.BodyContains))
                query = query.Where(p =>
                    EF.Functions.Like(p.Body, $"%{criteria.BodyContains.Trim()}%"));

            if (!string.IsNullOrWhiteSpace(criteria.Username))
                query = query.Where(p =>
                    p.User != null &&
                    p.User.Username == criteria.Username.Trim());

            if (criteria.CreatedAfter.HasValue)
                query = query.Where(p =>
                    p.CreatedAt >= criteria.CreatedAfter.Value);

            if (criteria.CreatedBefore.HasValue)
                query = query.Where(p =>
                    p.CreatedAt <= criteria.CreatedBefore.Value);

            if (!criteria.IncludeReplies)
                query = query.Where(p => p.ParentPostId == null);

            // apply ordering, paging
            return await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task SaveChangesAsync()
            => _db.SaveChangesAsync();
    }
}

