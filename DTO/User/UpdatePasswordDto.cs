using System.ComponentModel.DataAnnotations;

namespace backlogged_api.DTO.User
{
    public class UpdatePasswordDto
    {
        [Required]
        public required string CurrentPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
        [Required]
        public required string ConfirmNewPassword { get; set; }
    }
}