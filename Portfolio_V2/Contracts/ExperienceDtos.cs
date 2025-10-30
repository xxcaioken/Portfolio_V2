using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record ExperienceResponse(
        Guid Id,
        string Company,
        string Role,
        string StartDate,
        string? EndDate,
        string[] Bullets,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateExperienceRequest
    {
        [Required, MaxLength(150)] public string Company { get; set; } = string.Empty;
        [Required, MaxLength(120)] public string Role { get; set; } = string.Empty;
        [Required] public string StartDate { get; set; } = string.Empty; 
        public string? EndDate { get; set; } = null;
        [MinLength(0)] public string[] Bullets { get; set; } = [];
    }

    public class UpdateExperienceRequest : CreateExperienceRequest { }
}


