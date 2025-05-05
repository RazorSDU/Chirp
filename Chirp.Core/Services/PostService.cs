using System;
using Chirp.Core.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Specifications;

namespace Chirp.Core.Services
{
    public sealed class PostService : IPostService
    {
        private readonly IPostRepository _posts;
        private readonly IUserRepository _users;

        private const int MaxBodyLength = 280;

        public PostService(IPostRepository posts, IUserRepository users)
        {
            _posts = posts;
            _users = users;
        }

        /* ───────────────────────────────────────────────────
           1. GetAll – get the most recent posts in the feed            
        */

        public Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize)
            => _posts.GetRootPageAsync(page, pageSize);

        /* ───────────────────────────────────────────────────
           2. GetById – get a post by its ID             
        */

        public Task<Post?> GetByIdAsync(Guid id)
            => _posts.GetByIdAsync(id);

        /* ───────────────────────────────────────────────────
           3. GetByUser – get posts by a specific user               
        */

        public async Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username required", nameof(username));

            return await _posts.GetByUserAsync(username.Trim(), page, pageSize);
        }

        /* ───────────────────────────────────────────────────
           4. Create – Add a new post or comment to the feed               
        */

        public async Task<Post> CreateAsync(Guid userId, string body, Guid? parentPostId = null)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Body cannot be empty", nameof(body));

            if (body.Length > MaxBodyLength)
                throw new ArgumentException($"Body exceeds {MaxBodyLength} characters", nameof(body));

            var userExists = await _users.ExistsAsync(userId);
            if (!userExists)
                throw new InvalidOperationException("User not found");

            if (parentPostId is not null)
            {
                var parent = await _posts.GetByIdAsync(parentPostId.Value);
                if (parent is null)
                    throw new InvalidOperationException("Parent post not found");
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ParentPostId = parentPostId,
                Body = body.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _posts.AddAsync(post);
            await _posts.SaveChangesAsync();
            return post;
        }

        /* ───────────────────────────────────────────────────
           5. Update – edit an existing post                 
        */
        public async Task<Post> UpdateAsync(Guid id, string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("Body cannot be empty", nameof(body));

            if (body.Length > MaxBodyLength)
                throw new ArgumentException($"Body exceeds {MaxBodyLength} characters", nameof(body));

            var post = await _posts.GetByIdAsync(id);
            if (post is null)
                throw new InvalidOperationException("Post not found");

            post.Body = body.Trim();
            // if you tracked an UpdatedAt, set it here:
            // post.UpdatedAt = DateTime.UtcNow;

            // repository will track the change
            await _posts.SaveChangesAsync();
            return post;
        }

        /* ───────────────────────────────────────────────────
           6. Delete – remove a post                          
        */
        public async Task DeleteAsync(Guid id)
        {
            var post = await _posts.GetByIdAsync(id);
            if (post is null)
                throw new InvalidOperationException("Post not found");

            // 1) orphan any direct replies
            var replies = await _posts.GetRepliesAsync(id);
            foreach (var reply in replies)
            {
                reply.ParentPostId = null;
                _posts.Update(reply);
            }

            // 2) remove the post
            _posts.Remove(post);

            // 3) commit both changes in one save
            await _posts.SaveChangesAsync();
        }

        /* ───────────────────────────────────────────────────
           7. DeleteThread – remove a post, and all its children posts                         
        */
        public async Task DeleteThreadAsync(Guid id)
        {
            // 1) ensure the root post exists
            var root = await _posts.GetByIdAsync(id);
            if (root is null)
                throw new InvalidOperationException("Post not found");

            // 2) collect it and all descendants
            var toDelete = new List<Post>();
            var stack = new Stack<Post>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                toDelete.Add(current);

                // get direct replies (unpaged)
                var replies = await _posts.GetRepliesAsync(current.Id);
                foreach (var reply in replies)
                {
                    stack.Push(reply);
                }
            }

            // 3) remove them all
            foreach (var post in toDelete)
            {
                _posts.Remove(post);
            }

            // 4) commit
            await _posts.SaveChangesAsync();
        }

        /* ───────────────────────────────────────────────────
           8. Search - search for posts by Criteria                       
        */
        public Task<IEnumerable<Post>> SearchAsync(
            PostSearchCriteria criteria,
            int page,
            int pageSize)
        {
            // validation of page/pageSize omitted for brevity
            return _posts.SearchAsync(criteria, page, pageSize);
        }

        /* ───────────────────────────────────────────────────
           9. Update - Assign an image to a post                     
        */

        public async Task<Post> AssignImageAsync(Guid postId, Guid imageId)
        {
            var post = await _posts.GetByIdAsync(postId);
            if (post is null) throw new InvalidOperationException("Post not found");

            // you might optionally validate image exists via IImageRepository here

            post.ImageId = imageId;
            await _posts.SaveChangesAsync();
            return post;
        }
    }
}

