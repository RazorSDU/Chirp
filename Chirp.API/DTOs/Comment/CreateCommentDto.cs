using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.API.DTOs.Comment
{
    public class CreateCommentDto
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}
