using System.ComponentModel.DataAnnotations;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Contracts
{
    public record SocialLinkDto(string Label, string Url, string? IconKey);

    public record AboutResponse(
        string Name,
        string Title,
        string Summary,
        string Location,
        string Phone,
        string Email,
        string Linkedin,
        string? Github,
        string? AvatarUrl,
        string? FooterNote,
        SocialLinkDto[] Socials
    );

    public class UpdateAboutRequest
    {
        [Required, MaxLength(150)] public string Name { get; set; } = string.Empty;
        [Required, MaxLength(150)] public string Title { get; set; } = string.Empty;
        [MaxLength(2000)] public string Summary { get; set; } = string.Empty;
        [MaxLength(150)] public string Location { get; set; } = string.Empty;
        [MaxLength(50)] public string Phone { get; set; } = string.Empty;
        [MaxLength(200), EmailAddress] public string Email { get; set; } = string.Empty;
        [MaxLength(300), Url] public string Linkedin { get; set; } = string.Empty;
        [MaxLength(300), Url] public string? Github { get; set; }
        [MaxLength(500), Url] public string? AvatarUrl { get; set; }
        [MaxLength(300)] public string? FooterNote { get; set; }
        public SocialLinkDto[] Socials { get; set; } = Array.Empty<SocialLinkDto>();
    }
}


