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
        private readonly IPostService _posts;
        private readonly ICommentService _comments;
        private readonly IImageService _images;

        public CommentsController(
            IPostService posts,
            ICommentService comments,
            IImageService images)
        {
            _posts = posts;
            _comments = comments;
            _images = images;
        }

        // ────────────────────────────────────────────────────────
        // 0. Get a post’s image blob directly so Swagger can render it
        // ────────────────────────────────────────────────────────
        [HttpGet("{id:guid}/image")]
        [Produces("image/png", "image/jpeg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostImage(Guid id)
        {
            var post = await _posts.GetByIdAsync(id);
            if (post == null || post.ImageId == null)
                return NotFound();

            var img = await _images.GetByIdAsync(post.ImageId.Value);
            if (img == null)
                return NotFound();

            // returns binary with correct content‐type; Swagger UI will show the image inline
            return File(img.Data, img.ContentType!);
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
                comment.Replies?.Count ?? 0,
                comment.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = comment.Id },
                      protocol: Request.Scheme)
                : null);

            return Ok(result);
        }
    }
}
