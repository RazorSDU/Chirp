using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.API.DTOs.Post
{
    public class CreatePostDto
    {
        public Guid UserId { get; set; }
        public string Body { get; set; } = string.Empty;
    }
}
