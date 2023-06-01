namespace backlogged_api.Models
{
    public record Genre : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
