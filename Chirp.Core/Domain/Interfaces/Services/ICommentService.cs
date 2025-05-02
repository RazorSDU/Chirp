using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;

namespace Chirp.Core.Domain.Interfaces.Services;

public interface ICommentService
{
    /// <summary>Returns comments belonging to a post (paged).</summary>
    Task<IEnumerable<Post>> GetForPostAsync(Guid postId, int page, int pageSize);

    /// <summary>Add comment beneath a post.</summary>
    Task<Post> AddAsync(Guid postId, Guid userId, string body);
}