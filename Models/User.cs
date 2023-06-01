using Microsoft.AspNetCore.Identity;

namespace backlogged_api.Models
{
    public class User : IdentityUser<Guid>
    {
        public required string username { get; set; }
        public string? userName { get; set; }
        public string? lastName { get; set; }
        public required string email { get; set; }
        public DateTime registeredAt { get; set; } = DateTime.Now;
        public string? passwordHash { get; set; }
        public string? profileImageUrl { get; set; }
        public ICollection<Review>? reviews { get; set; }
        public Guid? backlogId { get; set; }
        public Backlog? backlog { get; set; }
    }
}
