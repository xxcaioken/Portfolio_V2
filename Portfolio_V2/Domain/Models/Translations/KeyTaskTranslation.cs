namespace Portfolio_V2.Domain.Models.Translations
{
    public class KeyTaskTranslation
    {
        public Guid Id { get; set; }
        public Guid KeyTaskBulletId { get; set; }
        public string KeyTask { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}


