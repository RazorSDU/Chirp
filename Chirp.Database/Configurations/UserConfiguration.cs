using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chirp.Database.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> e)
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            e.Property(u => u.Username)
                .HasColumnName("username")
                .HasMaxLength(32)
                .IsRequired();

            e.HasIndex(u => u.Username).IsUnique();

            e.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            e.Property(u => u.Role)
                .HasColumnName("role")
                .HasConversion<string>()                // enum ↔ varchar
                .HasDefaultValue(Role.User);
        }
    }

}
