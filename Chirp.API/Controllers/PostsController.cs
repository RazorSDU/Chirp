using Chirp.Core.Domain.Interfaces.Services;
using Chirp.API.DTOs.Post;
using Microsoft.AspNetCore.Mvc;
using Chirp.Core.Domain.Specifications;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Chirp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PostsController : ControllerBase
    {
        private readonly IPostService _posts;
        private readonly ICommentService _comments;
        private readonly IImageService _images;

        public PostsController(
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
           1. Feed – most‑recent “root” posts (no parent)          */

        // GET api/posts/feed?page=1&pageSize=20
        [HttpGet("feed")]
        public async Task<IActionResult> GetAllFeed(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var posts = await _posts.GetAllAsync(page, pageSize);

            var dtos = posts.Select(p => new PostDto(
                p.Id,
                p.User?.Username ?? string.Empty,
                p.Body,
                p.CreatedAt,
                p.ParentPostId,
                p.Replies?.Count ?? 0,
                p.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = p.Id },
                      protocol: Request.Scheme)
                : null));

            return Ok(dtos);
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

            var dtos = comments.Select(p => new PostDto(
                p.Id,
                p.User?.Username ?? string.Empty,
                p.Body,
                p.CreatedAt,
                p.ParentPostId,
                p.Replies?.Count ?? 0,
                p.ImageId != null
                    ? Url.Action(
                          action: nameof(GetPostImage),
                          controller: "Posts",
                          values: new { id = p.Id },
                          protocol: Request.Scheme)
                    : null));

            return Ok(dtos);
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
                replyCount,
                post.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = post.Id },
                      protocol: Request.Scheme)
                : null)
        };

            dto.AddRange(comments.Select(c => new PostDto(
                c.Id,
                c.User?.Username ?? string.Empty,
                c.Body,
                c.CreatedAt,
                c.ParentPostId,
                c.Replies?.Count ?? 0,
                c.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = c.Id },
                      protocol: Request.Scheme)
                : null)));

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
                p.Replies?.Count ?? 0,
                p.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = p.Id },
                      protocol: Request.Scheme)
                : null));

            return Ok(dto);
        }

        /* ────────────────────────────────────────────────────────
          5. Create – a new post                                   */

        // POST api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost(
            [FromBody] CreatePostDto dto)
        {
            var post = await _posts.CreateAsync(dto.UserId, dto.Body);

            var result = new PostDto(
                post.Id,
                post.User?.Username ?? string.Empty,
                post.Body,
                post.CreatedAt,
                post.ParentPostId,
                post.Replies?.Count ?? 0,
                post.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = post.Id },
                      protocol: Request.Scheme)
                : null);

            return Ok(result);
        }
        /* ────────────────────────────────────────────────────────
           6. Update – edit an existing post                        */

        // PUT api/posts/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(
            Guid id,
            [FromBody] UpdatePostDto dto)
        {
            // ensure post exists
            var existing = await _posts.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            // perform update
            var updated = await _posts.UpdateAsync(id, dto.Body);

            var result = new PostDto(
                updated.Id,
                updated.User?.Username ?? string.Empty,
                updated.Body,
                updated.CreatedAt,
                updated.ParentPostId,
                updated.Replies?.Count ?? 0,
                updated.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = updated.Id },
                      protocol: Request.Scheme)
                : null);

            return Ok(result);
        }

        /* ────────────────────────────────────────────────────────
           7. Delete – remove a post                                */

        // DELETE api/posts/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            // ensure post exists
            var existing = await _posts.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            // delete and return 204
            await _posts.DeleteAsync(id);
            return NoContent();
        }

        /* ───────────────────────────────────────────────────
           8. Delete Thread – remove a post and all replies    
        */

        // DELETE api/posts/{id}/thread
        [HttpDelete("{id:guid}/thread")]
        public async Task<IActionResult> DeleteThread(Guid id)
        {
            // quick exists check
            var existing = await _posts.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            await _posts.DeleteThreadAsync(id);
            return NoContent();
        }

        /* ───────────────────────────────────────────────────
           9. Search – find posts by body, dates, or username    
        */

        // GET api/posts/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? body,
            [FromQuery] string? username,
            [FromQuery] DateTime? createdAfter,
            [FromQuery] DateTime? createdBefore,
            [FromQuery] bool includeReplies = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var criteria = new PostSearchCriteria
            {
                BodyContains = body,
                Username = username,
                CreatedAfter = createdAfter,
                CreatedBefore = createdBefore,
                IncludeReplies = includeReplies
            };

            var posts = await _posts.SearchAsync(criteria, page, pageSize);

            var dto = posts.Select(p => new PostDto(
                p.Id,
                p.User?.Username ?? string.Empty,
                p.Body,
                p.CreatedAt,
                p.ParentPostId,
                p.Replies?.Count ?? 0,
                p.ImageId != null
                ? Url.Action(
                      action: nameof(GetPostImage),
                      controller: "Posts",
                      values: new { id = p.Id },
                      protocol: Request.Scheme)
                : null));

            return Ok(dto);
        }

        /* ───────────────────────────────────────────────────
            10. Upload – attach an image to a post - Use Postman to test this one.
        */
        // POST api/posts/{id}/image
        [HttpPost("{id:guid}/image")]
        [ApiExplorerSettings(IgnoreApi = true)]    // ← Swagger will skip this one
        public async Task<IActionResult> UploadPostImage(
        [FromRoute] Guid id,
        [FromForm(Name = "file")] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            var post = await _posts.GetByIdAsync(id);
            if (post is null) return NotFound();

            byte[] data;
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            data = ms.ToArray();

            var img = await _images.CreateAsync(data, file.FileName, file.ContentType);
            await _posts.AssignImageAsync(id, img.Id);

            var imageUrl = Url.Action(
                action: nameof(GetPostImage),
                controller: "Posts",
                values: new { id },
                protocol: Request.Scheme);

            return Ok(new { imageUrl });
        }

    }
}


