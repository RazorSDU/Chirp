using Chirp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chirp.Database.Configurations
{
    public sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> e)
        {
            e.ToTable("images");
            e.HasKey(i => i.Id);

            e.Property(i => i.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            e.Property(i => i.Data)
                .HasColumnName("data")
                .IsRequired();

            e.Property(i => i.Filename)
                .HasColumnName("filename")
                .HasMaxLength(255);

            e.Property(i => i.ContentType)
                .HasColumnName("content_type")
                .HasMaxLength(50);

            e.Property(i => i.UploadedAt)
                .HasColumnName("uploaded_at")
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
