using System.ComponentModel.DataAnnotations;

namespace backlogged_api.Models
{
    public record Review
    {
        public Guid id { get; set; }
        [Range(1, 5)]
        public int rating { get; set; }
        public string? details { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public Guid gameId { get; set; }
        public required Game game { get; set; }
        public Guid authorId { get; set; }
        public required User author { get; set; }    
    }
}
