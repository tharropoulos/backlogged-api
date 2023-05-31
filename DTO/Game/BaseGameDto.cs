using System.ComponentModel.DataAnnotations;

namespace backlogged_api.DTO.Game
{
    public class BaseGameDto
    {
        [Required]
        public required string Title { get; set; }
        public float Rating { get; set; }
        public string? BackgoundImageUrl { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public string? CoverImageUrl { get; set; }
        public Guid? FranchiseId { get; set; }
        public Guid? PublisherId { get; set; }

    }
}