using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.API.DTOs.Post
{
    public record PostDto
    (
    Guid Id,
    string Username,
    string Body,
    DateTime CreatedAt,
    Guid? ParentPostId,
    int ReplyCount,
    string? ImageUrl
    );
}
