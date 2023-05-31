using System.ComponentModel.DataAnnotations;

namespace backlogged_api.Models
{
    public record Review : BaseEntity
    {
        [Range(1, 5)]
        public int rating { get; set; }
        public string? details { get; set; }
        public DateTime createdAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        public Guid gameId { get; set; }
        public Game? game { get; set; }
        public Guid authorId { get; set; }
        public User? author { get; set; }
    }
}
