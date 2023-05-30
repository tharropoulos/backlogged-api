namespace backlogged_api.DTO.Review
{
    public class ReviewDto : BaseReviewDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public Guid GameId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
}