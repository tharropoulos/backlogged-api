using backlogged_api.Models;
using Microsoft.AspNetCore.Identity;

namespace backlogged_api.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? ProfileImageUrl { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public Guid? BacklogId { get; set; }
        public Backlog? Backlog { get; set; }
    }
}