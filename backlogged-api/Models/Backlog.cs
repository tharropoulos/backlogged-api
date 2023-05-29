namespace backlogged_api.Models
{
    public record Backlog
    {
        public Guid id { get; set; }
        public required Guid userId { get; set; }
        public required User user { get; set; }
        public ICollection<Game>? games { get; set; }
        public Boolean isVisible { get; set; } = false;

    }
}
