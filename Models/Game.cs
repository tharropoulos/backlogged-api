namespace backlogged_api.Models
{
    public record Game
    {
        public Guid id { get; set; }
        public required string title { get; set; }
        public string? description { get; set; }
        public float rating { get; set; }
        public string? coverImageUrl { get; set; }
        public string? backgroundImageUrl { get; set; }
        public Guid? franchiseId { get; set; }
        public Franchise? franchise { get; set; }
        public Guid? publisherId { get; set; }
        public Publisher? publisher { get; set; }
        public ICollection<Genre>? genres { get; set; }
        public ICollection<Developer>? developers { get; set; }
        public ICollection<Platform>? platforms { get; set; }
        public ICollection<Review>? reviews { get; set; }
        public ICollection<Backlog>? backlogs { get; set; } 
    }
}
