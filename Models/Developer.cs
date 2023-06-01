namespace backlogged_api.Models
{
    public record Developer : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}
