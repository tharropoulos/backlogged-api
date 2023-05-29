
using System.Diagnostics.CodeAnalysis;

namespace backlogged_api.DTO.Developer
{
    public class BaseDeveloperDto
    {
        [NotNull] public required string name { get; set; }
    }
}