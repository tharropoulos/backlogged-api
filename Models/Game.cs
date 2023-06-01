namespace backlogged_api.Models
{
    public record Game : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public float Rating { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public string? BackgroundImageUrl { get; set; }
        public Guid? FranchiseId { get; set; }
        public Franchise? Franchise { get; set; }
        public Guid? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<Genre>? Genres { get; set; }
        public ICollection<Developer>? Developers { get; set; }
        public ICollection<Platform>? Platforms { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Backlog>? Backlogs { get; set; }
    }
}
