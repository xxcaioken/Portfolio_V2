namespace Portfolio_V2.Domain.Models.Translations
{
    public class TestimonialTranslation
    {
        public Guid Id { get; set; }
        public Guid TestimonialId { get; set; }
        public string? Name { get; set; }
        public string? Highlight { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}



