using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;

namespace Chirp.Core.Domain.Interfaces.Services
{
    /// <summary>
    /// Post‑level use‑cases exposed to the API layer. No EF Core or infrastructure dependencies.
    /// </summary>
    public interface IPostService
    {
        /// <summary>Returns a page of root‑level posts (no ParentPostId).</summary>
        Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize);

        /// <summary>Returns a single post or <c>null</c>.</summary>
        Task<Post?> GetByIdAsync(Guid id);

        /// <summary>Creates a new post or comment.</summary>
        Task<Post> CreateAsync(Guid userId, string body, Guid? parentPostId = null);

        Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize);
    }
}


