using BCrypt.Net;
using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Database.Seed
{
    public static class Seeder
    {
        public static async Task SeedAsync(ChirpContext ctx)
        {
            if (await ctx.Users.AnyAsync()) return;

            var alice = new User
            {
                Id = Guid.NewGuid(),
                Username = "alice",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass123"),
                Role = Role.User
            };

            var bob = new User
            {
                Id = Guid.NewGuid(),
                Username = "bob",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("hunter2"),
                Role = Role.Moderator
            };

            var tweet = new Post
            {
                Id = Guid.NewGuid(),
                UserId = alice.Id,
                Body = "Hello Chirp!"
            };

            ctx.Users.AddRange(alice, bob);
            ctx.Posts.Add(tweet);

            await ctx.SaveChangesAsync();
        }
    }

}
