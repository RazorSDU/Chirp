using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Core.Domain.Entities
{
    public enum Role { User = 0, Moderator = 1, Admin = 2 }

    public sealed class User
    {
        public Guid Id { get; init; }           // PK
        public string Username { get; set; } = "";      // unique, ≤32 chars
        public string PasswordHash { get; set; } = "";      // never store raw pw
        public Role Role { get; set; } = Role.User;
        public ICollection<Post> Posts { get; init; } = new List<Post>();
    }

}
