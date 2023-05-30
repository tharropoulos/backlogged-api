namespace backlogged_api.DTO.Review
{
    public class CreateReviewDto : BaseReviewDto
    {
        public Guid AuthorId { get; set; }
        public Guid GameId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}