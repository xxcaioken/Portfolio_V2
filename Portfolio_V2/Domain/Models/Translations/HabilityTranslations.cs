namespace Portfolio_V2.Domain.Models.Translations
{
    public class HabilityItemTranslation
    {
        public Guid Id { get; set; }
        public Guid HabilityItemId { get; set; }
        public string Hability { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class HabilityBulletTranslation
    {
        public Guid Id { get; set; }
        public Guid HabilityBulletId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}


