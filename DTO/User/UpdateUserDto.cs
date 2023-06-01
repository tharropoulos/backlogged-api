namespace backlogged_api.DTO.User
{
    public record UpdateUserDto : BaseUserDto
    {
        public string? ProfileImageUrl { get; set; }

    }
}