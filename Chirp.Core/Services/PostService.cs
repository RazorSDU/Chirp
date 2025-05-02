using System;
using Chirp.Core.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Interfaces.Repositories;

namespace Chirp.Core.Services;

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

    public Task<IEnumerable<Post>> GetAllAsync(int page, int pageSize)
        => _posts.GetRootPageAsync(page, pageSize);

    public Task<Post?> GetByIdAsync(Guid id)
        => _posts.GetByIdAsync(id);

    public async Task<IEnumerable<Post>> GetByUserAsync(string username, int page, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username required", nameof(username));

        return await _posts.GetByUserAsync(username.Trim(), page, pageSize);
    }

    public async Task<Post> CreateAsync(Guid userId, string body, Guid? parentPostId = null)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be empty", nameof(body));

        if (body.Length > MaxBodyLength)
            throw new ArgumentException($"Body exceeds {MaxBodyLength} characters", nameof(body));

        var userExists = true; //await _users.ExistsAsync(userId);
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
}