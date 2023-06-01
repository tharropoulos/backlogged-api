using System.ComponentModel.DataAnnotations;

namespace backlogged_api.DTO.User
{
    public record CreateUserDto : BaseUserDto
    {
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string ConfirmPassword { get; set; }
        [Required]
        public required string Email { get; set; }
    }
}