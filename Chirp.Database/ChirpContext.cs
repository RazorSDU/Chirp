using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Database
{
    public class ChirpContext : DbContext
    {
        public ChirpContext(DbContextOptions<ChirpContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Image> Images => Set<Image>();

        protected override void OnModelCreating(ModelBuilder b)
            => b.ApplyConfigurationsFromAssembly(typeof(ChirpContext).Assembly);
    }


}
