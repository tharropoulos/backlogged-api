namespace backlogged_api.Models
{
    public class Game
    {
        public Guid id { get; set; }
        public required string title { get; set; }
        public  string? description { get; set; }
        public float rating { get; set; }
        public string? coverImageUrl { get; set; }
        public string? backgroundImageUrl { get; set; }
        public Guid? franchiseId { get; set; }
        public Franchise? franchise { get; set; }



    }
}
