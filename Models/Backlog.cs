namespace backlogged_api.Models
{
    public record Backlog : BaseEntity
    {
        public required Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Game>? Games { get; set; }
        public Boolean IsVisible { get; set; } = false;

    }
}
