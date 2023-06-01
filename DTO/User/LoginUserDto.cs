using System.ComponentModel.DataAnnotations;

namespace backlogged_api.DTO.User
{
    public record LoginUserDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}