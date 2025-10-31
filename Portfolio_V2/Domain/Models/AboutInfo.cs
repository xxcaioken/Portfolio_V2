namespace Portfolio_V2.Domain.Models
{
    public class AboutInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Linkedin { get; set; } = string.Empty;
        public string? Github { get; set; }
        public string? AvatarUrl { get; set; }
        public string? FooterNote { get; set; }
        public ICollection<SocialLink> Socials { get; set; } = new List<SocialLink>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public class SocialLink
    {
        public Guid Id { get; set; }
        public Guid AboutInfoId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? IconKey { get; set; }
        public AboutInfo? About { get; set; }
    }
}


