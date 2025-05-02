using Chirp.Core.Domain.Interfaces.Services;
using Chirp.API.DTOs.Post;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PostsController : ControllerBase
    {
        private readonly IPostService _posts;
        private readonly ICommentService _comments;

        public PostsController(IPostService posts, ICommentService comments)
        {
            _posts = posts;
            _comments = comments;
        }

        /* ────────────────────────────────────────────────────────
           1. Feed – most‑recent “root” posts (no parent)          */

        // GET api/posts/feed?page=1&pageSize=20
        [HttpGet("feed")]
        public async Task<IActionResult> GetAllFeed(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var posts = await _posts.GetAllAsync(page, pageSize);

            var dto = posts.Select(p => new PostDto(
                p.Id,
                p.User?.Username ?? string.Empty,
                p.Body,
                p.CreatedAt,
                p.ParentPostId,
                p.Replies?.Count ?? 0));

            return Ok(dto);
        }

        /* ────────────────────────────────────────────────────────
           2. Comments for a post (one layer)                      */

        // GET api/posts/{id}/comments?page=1&pageSize=50
        [HttpGet("{id:guid}/comments")]
        public async Task<IActionResult> GetPostCommentsById(
            Guid id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var comments = await _comments.GetForPostAsync(id, page, pageSize);

            var dto = comments.Select(c => new PostDto(
                c.Id,
                c.User?.Username ?? string.Empty,
                c.Body,
                c.CreatedAt,
                c.ParentPostId,
                c.Replies?.Count ?? 0));

            return Ok(dto);
        }

        /* ────────────────────────────────────────────────────────
           3. Thread – the post + its immediate comments           */

        // GET api/posts/{id}/thread?page=1&pageSize=50
        [HttpGet("{id:guid}/thread")]
        public async Task<IActionResult> GetPostAndCommentsById(
            Guid id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var post = await _posts.GetByIdAsync(id);
            if (post is null) return NotFound();

            var comments = await _comments.GetForPostAsync(id, page, pageSize);
            var replyCount = comments.Count();          // ←  direct replies we just loaded

            var dto = new List<PostDto>
        {
            new PostDto(
                post.Id,
                post.User?.Username ?? string.Empty,
                post.Body,
                post.CreatedAt,
                post.ParentPostId,
                replyCount)                         // ← use accurate count
        };

            dto.AddRange(comments.Select(c => new PostDto(
                c.Id,
                c.User?.Username ?? string.Empty,
                c.Body,
                c.CreatedAt,
                c.ParentPostId,
                c.Replies?.Count ?? 0)));

            return Ok(dto);
        }

        /* ────────────────────────────────────────────────────────
           4. Posts by a user                                      */

        // GET api/posts/user/{username}?page=1&pageSize=20
        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetPostsByUser(
            string username,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var posts = await _posts.GetByUserAsync(username, page, pageSize);

            var dto = posts.Select(p => new PostDto(
                p.Id,
                p.User?.Username ?? string.Empty,
                p.Body,
                p.CreatedAt,
                p.ParentPostId,
                p.Replies?.Count ?? 0));

            return Ok(dto);
        }
    }
}


