namespace backlogged_api.Models
{
    public record Publisher : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
