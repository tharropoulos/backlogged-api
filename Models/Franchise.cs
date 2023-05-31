namespace backlogged_api.Models
{
    public record Franchise : BaseEntity
    {
        public required string name { get; set; }
        public ICollection<Game>? games { get; set; }
    }
}
