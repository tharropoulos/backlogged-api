namespace backlogged_api.Helpers
{
    public class GameSearchParams
    {
        public string? Title { get; set; }
        public IEnumerable<Guid>? GenreIds { get; set; }
        public IEnumerable<Guid>? PlatformIds { get; set; }
        public IEnumerable<Guid>? DeveloperIds { get; set; }
        public IEnumerable<Guid>? PublisherIds { get; set; }
        public IEnumerable<Guid>? FranchiseIds { get; set; }
        public string? sortOrder { get; set; }

    }
}