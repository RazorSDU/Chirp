using Chirp.API.DTOs.Comment;
using Chirp.API.DTOs.Post;
using Chirp.Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CommentsController : ControllerBase
    {
        private readonly ICommentService _comments;
        private readonly IPostService _posts;

        public CommentsController(ICommentService comments, IPostService posts)
        {
            _comments = comments;
            _posts = posts;
        }

        /* ────────────────────────────────────────────────────────
           1. Add – a comment to a post                              */

        // POST api/comments
        [HttpPost]
        public async Task<IActionResult> AddComment(
            [FromBody] CreateCommentDto dto)
        {
            // ensure the parent post exists
            var parent = await _posts.GetByIdAsync(dto.PostId);
            if (parent is null) return NotFound();

            var comment = await _comments.AddAsync(dto.PostId, dto.UserId, dto.Body);

            var result = new PostDto(
                comment.Id,
                comment.User?.Username ?? string.Empty,
                comment.Body,
                comment.CreatedAt,
                comment.ParentPostId,
                comment.Replies?.Count ?? 0);

            return Ok(result);
        }
    }
}
