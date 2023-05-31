namespace backlogged_api.Models
{
    public record Publisher : BaseEntity
    {
        public required string name { get; set; }
        public ICollection<Game>? games { get; set; }
    }
}
