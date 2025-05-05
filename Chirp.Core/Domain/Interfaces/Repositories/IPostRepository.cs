using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Specifications;

namespace Chirp.Core.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Read/write access for <see cref="Post"/> aggregate – no EF Core here.
    /// </summary>
    public interface IPostRepository
    {
        // ── queries ────────────────────────────────────────────
        Task<Post?> GetByIdAsync(Guid id);

        /// <summary>Root‑level posts (ParentPostId == null), paged.</summary>
        Task<IEnumerable<Post>> GetRootPageAsync(int page, int pageSize);

        /// <summary>Replies for a post, paged.</summary>
        Task<IEnumerable<Post>> GetRepliesPageAsync(Guid postId, int page, int pageSize);

        /// <summary>Replies for a post, NOT paged.</summary>
        Task<IEnumerable<Post>> GetRepliesAsync(Guid parentPostId);

        /// <summary>All posts written by <paramref name="username"/>, paged &amp; newest first.</summary>
        Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize);

        // ── commands ───────────────────────────────────────────
        Task AddAsync(Post post);
        void Update(Post post);
        void Remove(Post post);
        Task<IEnumerable<Post>> SearchAsync(
            PostSearchCriteria criteria,
            int page,
            int pageSize);

        /// <summary>Persists all pending changes.</summary>
        Task SaveChangesAsync();
    }
}

