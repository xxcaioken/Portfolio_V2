namespace Portfolio_V2.Domain.Models
{
    public class HabilityBullet
    {
        public Guid Id { get; set; }
        public Guid HabilityItemId { get; set; }
        public required string Text { get; set; }
        public string? Badge { get; set; }

        public HabilityItem? HabilityItem { get; set; }
    }
}


