using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace backlogged_api.DTO.Review
{
    public class BaseReviewDto
    {
        public string? Details { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}