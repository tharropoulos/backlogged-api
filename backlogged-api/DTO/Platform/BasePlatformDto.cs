using System.Diagnostics.CodeAnalysis;

namespace backlogged_api.DTO.Platform
{
    public class BasePlatformDto
    {
        [NotNull] public required string name { get; set; }
    }
}