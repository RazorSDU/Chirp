using Chirp.Core.Domain.Entities;

namespace Chirp.Core.DTOs
{
    public record UserDto
    {
        public Guid Id  { get; set; }
        public string Username  { get; set; }
        public ICollection<Post> Posts { get; set; }
    };

    public record CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public record UpdateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}