namespace backlogged_api.Models
{
    public record Franchise : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
