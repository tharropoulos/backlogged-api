namespace backlogged_api.Models
{
    public record Backlog : BaseEntity
    {
        public required Guid userId { get; set; }
        public required User user { get; set; }
        public ICollection<Game>? games { get; set; }
        public Boolean isVisible { get; set; } = false;

    }
}
