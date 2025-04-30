using System;
using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chirp.Database.Configurations
{
    public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> e)
        {
            e.ToTable("posts");
            e.HasKey(p => p.Id);

            e.Property(u => u.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWSEQUENTIALID()");


            e.Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            e.Property(p => p.ParentPostId)
                .HasColumnName("parent_post_id");

            e.Property(p => p.Body)
                .HasColumnName("body")
                .HasMaxLength(280)
                .IsRequired();

            e.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            e.Property(p => p.UpdatedAt).HasColumnName("updated_at");
            e.Property(p => p.DeletedAt).HasColumnName("deleted_at");

            // ── relationships ─────────────────────────────────────────────
            e.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.ParentPost)
                .WithMany(p => p.Replies)
                .HasForeignKey(p => p.ParentPostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }

}
