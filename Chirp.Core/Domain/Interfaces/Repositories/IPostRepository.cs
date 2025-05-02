using Chirp.Core.Domain.Entities;

namespace Chirp.Core.Domain.Interfaces.Repositories;

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

    /// <summary>All posts written by <paramref name="username"/>, paged &amp; newest first.</summary>
    Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize);

    // ── commands ───────────────────────────────────────────
    Task AddAsync(Post post);

    /// <summary>Persists all pending changes.</summary>
    Task SaveChangesAsync();
}