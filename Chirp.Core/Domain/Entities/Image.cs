using System;
using System.Collections.Generic;

namespace Chirp.Core.Domain.Entities
{
    public sealed class Image
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; } = null!;       // BLOB
        public string? Filename { get; set; }
        public string? ContentType { get; set; }               // e.g. image/png
        public DateTime UploadedAt { get; set; }               // timestamptz default now()

        // navigation back to any posts that reference this image
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
