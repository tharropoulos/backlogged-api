
using backlogged_api.DTO.Game;
using backlogged_api.Helpers;

namespace backlogged_api.DTO.Backlog
{
    public record UpdateBacklogDto
    {
        public Boolean IsVisible { get; set; } = false;

    }
}