using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Repositories;
using Chirp.Core.Domain.Specifications;
using Chirp.Core.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Chirp.Core.Tests;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _posts = new();
    private readonly Mock<IUserRepository> _users = new();

    private readonly PostService _sut;

    public PostServiceTests()
    {
        _sut = new PostService(_posts.Object, _users.Object);
    }

    /* ----------------------------------------------------- */
    [Theory]
    [InlineData(null)]
    [InlineData("")]    // empty
    [InlineData("   ")] // whitespace
    public async Task CreateAsync_body_required(string body)
    {
        Func<Task> act = () => _sut.CreateAsync(Guid.NewGuid(), body, null);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Body cannot be empty*");
    }

    [Fact]
    public async Task CreateAsync_body_max_280()
    {
        var longBody = new string('x', 281);
        Func<Task> act = () => _sut.CreateAsync(Guid.NewGuid(), longBody, null);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Body exceeds 280*");
    }

    [Fact]
    public async Task CreateAsync_user_must_exist()
    {
        _users.Setup(u => u.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        Func<Task> act = () => _sut.CreateAsync(Guid.NewGuid(), "hi", null);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User not found");
    }

    [Fact]
    public async Task CreateAsync_parent_post_must_exist()
    {
        var uid = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        _users.Setup(u => u.ExistsAsync(uid)).ReturnsAsync(true);
        _posts.Setup(p => p.GetByIdAsync(parentId)).ReturnsAsync((Post?)null);

        Func<Task> act = () => _sut.CreateAsync(uid, "reply", parentId);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Parent post not found");
    }

    [Fact]
    public async Task CreateAsync_success_returns_new_post()
    {
        var uid = Guid.NewGuid();
        _users.Setup(u => u.ExistsAsync(uid)).ReturnsAsync(true);
        _posts.Setup(p => p.AddAsync(It.IsAny<Post>()))
              .Returns(Task.CompletedTask);
        _posts.Setup(p => p.SaveChangesAsync())
              .Returns(Task.CompletedTask);

        var post = await _sut.CreateAsync(uid, "hello world", null);

        post.Body.Should().Be("hello world");
        post.UserId.Should().Be(uid);
        _posts.Verify(p => p.AddAsync(It.Is<Post>(x => x.Id == post.Id)), Times.Once);
        _posts.Verify(p => p.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_passes_through_to_repository()
    {
        var expected = new List<Post> { new() { Id = Guid.NewGuid() } };
        _posts.Setup(p => p.GetRootPageAsync(1, 10)).ReturnsAsync(expected);

        var result = await _sut.GetAllAsync(1, 10);
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task GetByIdAsync_passes_through_to_repository()
    {
        var id = Guid.NewGuid();
        var expected = new Post { Id = id };
        _posts.Setup(p => p.GetByIdAsync(id)).ReturnsAsync(expected);

        var result = await _sut.GetByIdAsync(id);
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task GetByUserAsync_calls_repository()
    {
        var expected = new List<Post>();
        _posts.Setup(p => p.GetByUserAsync("alice", 1, 20))
              .ReturnsAsync(expected);

        var result = await _sut.GetByUserAsync("alice", 1, 20);
        result.Should().BeSameAs(expected);
    }
}
