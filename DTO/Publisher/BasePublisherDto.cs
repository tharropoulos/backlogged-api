using System.Diagnostics.CodeAnalysis;

namespace backlogged_api.DTO.Publisher
{
    public class BasePublisherDto
    {
        [NotNull] public required string name { get; set; }
    }
}
