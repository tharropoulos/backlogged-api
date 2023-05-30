
using System.Diagnostics.CodeAnalysis;

namespace backlogged_api.DTO.Genre
{
    public class BaseGenreDto
    {
        [NotNull] public required string name { get; set; }
    }
}