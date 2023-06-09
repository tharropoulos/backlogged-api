using backlogged_api.DTO.Game;
using backlogged_api.Helpers;

namespace backlogged_api.DTO.Backlog
{
    public record BacklogDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Boolean IsVisible { get; set; } = false;

    }
}