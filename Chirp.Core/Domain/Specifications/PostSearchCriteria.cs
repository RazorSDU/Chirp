// Chirp.Core.Domain.Specifications/PostSearchCriteria.cs
using System;

namespace Chirp.Core.Domain.Specifications
{
    /// <summary>
    /// Encapsulates filtering options for searching posts.
    /// Add new properties here when you need new filters.
    /// </summary>
    public class PostSearchCriteria
    {
        public string? BodyContains { get; set; }
        public string? Username { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public bool IncludeReplies { get; set; } = true;
    }
}
