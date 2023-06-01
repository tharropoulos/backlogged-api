namespace backlogged_api.DTO.User
{
    public record UserDto : BaseUserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public bool? TwoFactorEnabled { get; set; } = false;
        public bool? EmailConfirmed { get; set; } = false;
        public int AccessFailedCount { get; set; } = 0;
        public Guid BacklogId { get; set; }

    }
}