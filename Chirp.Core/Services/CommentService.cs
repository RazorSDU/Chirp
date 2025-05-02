using Chirp.Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;

namespace Chirp.Core.Services
{
    public sealed class CommentService : ICommentService
    {
        private readonly IPostRepository _posts;
        private readonly IPostService _postService;   // reuse business validation

        public CommentService(IPostRepository posts, IPostService postService)
        {
            _posts = posts;
            _postService = postService;
        }

        public Task<IEnumerable<Post>> GetForPostAsync(Guid postId, int page, int pageSize)
            => _posts.GetRepliesPageAsync(postId, page, pageSize);

        public Task<Post> AddAsync(Guid postId, Guid userId, string body)
            => _postService.CreateAsync(userId, body, postId);
    }
}


