namespace backlogged_api.DTO.Game
{
    public class GameDto : BaseGameDto
    {

        public Guid Id { get; set; }
        public Guid? FranchiseId { get; set; }
        public Guid? PublisherId { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

    }
}