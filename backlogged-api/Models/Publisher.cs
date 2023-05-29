namespace backlogged_api.Models
{
    public record Publisher
    {
        public Guid id { get; set; }
        public required string name { get; set; }
        public ICollection<Game>? games { get; set; }
    }
}
