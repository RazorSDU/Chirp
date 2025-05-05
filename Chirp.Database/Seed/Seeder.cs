using BCrypt.Net;
using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Database.Seed;

public static class Seeder
{
    public static async Task SeedAsync(ChirpContext ctx)
    {
        await ctx.Database.BeginTransactionAsync();
        try
        {
            // ── 1) Wipe existing data ───────────────────────────
            // Must delete posts first (FK→images), then images, then users
            await ctx.Posts.ExecuteDeleteAsync();
            await ctx.Images.ExecuteDeleteAsync();
            await ctx.Users.ExecuteDeleteAsync();

            // ── 2) Seed users ───────────────────────────────────
            var users = new[]
            {
                    CreateUser("alice", "pass123", Role.User),
                    CreateUser("bob",   "hunter2", Role.Moderator)
                };
            await ctx.Users.AddRangeAsync(users);

            // ── 3) Seed placeholder image ───────────────────────
            // Make sure "Seed/Images/placeholder.png" is copied to output dir
            var imagePath = Path.Combine("Images", "placeholder.png");
            var placeholderImage = new Image
            {
                Id = Guid.NewGuid(),
                Data = ImageConverter.ConvertToBytes(imagePath),
                Filename = "placeholder.png",
                ContentType = "image/png",
                UploadedAt = DateTime.UtcNow
            };
            await ctx.Images.AddAsync(placeholderImage);

            // ── 4) Seed posts & wire up some to the placeholder ──
            var posts = CreatePosts(users);
            // assign the placeholder to ~25% of posts
            var rnd = new Random(1234);
            foreach (var (post, idx) in posts.Select((p, i) => (p, i)))
            {
                if (idx % 4 == 0)
                    post.ImageId = placeholderImage.Id;
            }
            await ctx.Posts.AddRangeAsync(posts);

            // ── 5) Commit all ───────────────────────────────────
            await ctx.SaveChangesAsync();
            await ctx.Database.CommitTransactionAsync();
        }
        catch
        {
            await ctx.Database.RollbackTransactionAsync();
            throw;
        }
    }

    /* ------------------------------------------------------- */

    private static User CreateUser(string username, string pwd, Role role) =>
        new()
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(pwd),
            Role = role
        };

    /*  Synthetic thread data 
        Each tuple: Key, Body, ParentKey (null = root)              */
    private static readonly (string Key, string Body, string? ParentKey)[] Thread =
    [
        ("P1",  "Excited to join Chirp!",                 null),
        ("P4",  "Thanks everyone for the warm welcome!",  "P1"),
        ("P7",  "You're all amazing!",                    "P4"),
        ("P11", "Appreciate the support.",                "P7"),
        ("P16", "This community rocks.",                  "P11"),
        ("P17", "Happy to be here!",                      "P11"),
        ("P8",  "Let's make Chirp great.",                "P4"),
        ("P12", "Agreed, positivity matters.",            "P8"),
        ("P13", "Looking forward to learning more.",      "P8"),
        ("P18", "Can't wait to share more updates.",      "P13"),
        ("P21", "Stay tuned for exciting news!",          "P18"),
        ("P5",  "How do I customize my feed?",            "P1"),
        ("P9",  "Try following more people.",             "P5"),
        ("P14", "The explore page is also great!",        "P9"),
        ("P6",  "Is there a mobile app coming?",          "P1"),
        ("P10", "I hope so, that’d be awesome.",          "P6"),
        ("P15", "Crossing fingers for dark mode too!",    "P6"),

        ("P2",  "Anyone else new here?",                  null),
        ("P19", "Yes! Just signed up yesterday.",         "P2"),
        ("P22", "Welcome! Need any help?",                "P19"),
        ("P25", "Where can I find trending posts?",       "P22"),
        ("P28", "Check the top right corner menu.",       "P25"),
        ("P26", "What’s your favorite Chirp feature?",    "P22"),
        ("P29", "Definitely the nested replies!",         "P26"),
        ("P20", "This app feels better than others.",     "P2"),
        ("P23", "It’s the simplicity for me.",            "P20"),
        ("P30", "Yeah, very clean UI.",                   "P23"),
        ("P33", "Hope it stays that way.",                "P30"),
        ("P24", "I like the support team.",               "P20"),
        ("P27", "Same, very responsive.",                 "P24"),
        ("P31", "They even fixed a bug I reported!",      "P24"),
        ("P32", "Big shoutout to the devs!",              "P2"),

        ("P3",  "Daily thought: small wins count.",       null),
        ("P34", "Totally agree. Progress > Perfection.",  "P3"),
        ("P36", "Even 1% improvement matters.",           "P34"),
        ("P39", "Compound growth is powerful!",           "P36"),
        ("P37", "Thanks for the reminder!",               "P34"),
        ("P40", "Needed to hear that today.",             "P37"),
        ("P35", "Chirp should have goal tracking!",       "P3"),
        ("P38", "Ooh, that’s a cool idea.",               "P35"),
        ("P41", "Like a progress badge system?",          "P38"),
        ("P42", "Gamification would boost engagement!",   "P38"),
        ("P43", "How do we submit feature requests?",     "P3"),
        ("P44", "There’s a form in the help center.",     "P43")
    ];

    private static List<Post> CreatePosts(User[] users)
    {
        var now = DateTime.UtcNow;
        var rnd = new Random();
        var userPicker = () => users[rnd.Next(users.Length)];

        DateTime RandomizedDate()
        {
            int monthsToShift = rnd.Next(-3, 4); // -3 to +3 inclusive
            return now.AddMonths(monthsToShift);
        }

        // First pass – create Post objects without ParentPostId
        var keyToPost = Thread.ToDictionary(
            t => t.Key,
            t => new Post
            {
                Id = Guid.NewGuid(),
                UserId = userPicker().Id,
                Body = t.Body,
                CreatedAt = RandomizedDate()
            });

        // Second pass – wire up Parents
        foreach (var (key, _, parentKey) in Thread)
        {
            if (parentKey is not null && keyToPost.TryGetValue(parentKey, out var parent))
                keyToPost[key].ParentPostId = parent.Id;
        }

        return keyToPost.Values.ToList();
    }
}
