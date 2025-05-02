using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.API.DTOs.Comment
{
    public record CommentDto
    (
    Guid Id,
    string Username,
    string Body,
    DateTime CreatedAt,
    Guid ParentPostId
    );
}
