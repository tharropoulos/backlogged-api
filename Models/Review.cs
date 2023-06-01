using System.ComponentModel.DataAnnotations;

namespace backlogged_api.Models
{
    public record Review : BaseEntity
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Details { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        public Guid GameId { get; set; }
        public Game Game { get; set; }
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
    }
}
