using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record ExperienceResponse(
        Guid Id,
        string Company,
        string Role,
        string Period,
        string[] Bullets,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateExperienceRequest
    {
        [Required, MaxLength(150)] public string Company { get; set; } = string.Empty;
        [Required, MaxLength(120)] public string Role { get; set; } = string.Empty;
        [Required, MaxLength(120)] public string Period { get; set; } = string.Empty;
        [MinLength(0)] public string[] Bullets { get; set; } = Array.Empty<string>();
    }

    public class UpdateExperienceRequest : CreateExperienceRequest { }
}


