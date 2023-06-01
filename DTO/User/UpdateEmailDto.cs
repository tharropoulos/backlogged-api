namespace backlogged_api.DTO.User
{
    public record UpdateEmailDto
    {
        public string? CurrentPassword { get; set; }
        public string? NewEmail { get; set; }
    }
}