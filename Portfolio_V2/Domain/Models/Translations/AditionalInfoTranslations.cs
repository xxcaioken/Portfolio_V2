namespace Portfolio_V2.Domain.Models.Translations
{
    public class AditionalInfoItemTranslation
    {
        public Guid Id { get; set; }
        public Guid AditionalInfoItemId { get; set; }
        public string AditionalInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class AditionalInfoBulletTranslation
    {
        public Guid Id { get; set; }
        public Guid AditionalInfoBulletId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}


