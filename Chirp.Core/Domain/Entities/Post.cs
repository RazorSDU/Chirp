using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Core.Domain.Entities
{
    public sealed class Post
    {
        public Guid Id { get; init; }
        public Guid UserId { get; set; }
        public Guid? ParentPostId { get; set; }         // reply thread
        public string Body { get; set; } = "";
        public int ReplyCount { get; set; } = 0; // number of replies to this post
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public User? User { get; set; }
        public Post? ParentPost { get; set; }
        public ICollection<Post> Replies { get; init; } = new List<Post>();

        // ── new image FK ────────────────────────────────────────
        public Guid? ImageId { get; set; }
        public Image? Image { get; set; }
    }

}
