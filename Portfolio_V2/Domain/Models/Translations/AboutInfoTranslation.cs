namespace Portfolio_V2.Domain.Models.Translations
{
    public class AboutInfoTranslation
    {
        public Guid Id { get; set; }
        public Guid AboutInfoId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? FooterNote { get; set; }
        public ICollection<SocialLinkTranslation> Socials { get; set; } = new List<SocialLinkTranslation>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class SocialLinkTranslation
    {
        public Guid Id { get; set; }
        public Guid AboutInfoTranslationId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? IconKey { get; set; }
    }
}


