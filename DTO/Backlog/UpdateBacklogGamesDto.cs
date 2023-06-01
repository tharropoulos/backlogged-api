namespace backlogged_api.DTO.Backlog
{
    public record UpdateBacklogGamesDto
    {
        public string Operation { get; set; } = "add";
        public IEnumerable<Guid>? GameIds { get; set; }
    }
}