namespace Portfolio_V2.Domain.Models
{
    public class RecommendationLetter
    {
        public Guid Id { get; set; }
        public string? ImageUrlPt { get; set; }
        public string? ImageUrlEn { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}


