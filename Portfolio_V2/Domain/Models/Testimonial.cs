namespace Portfolio_V2.Domain.Models
{
    public class Testimonial
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Highlight { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}



