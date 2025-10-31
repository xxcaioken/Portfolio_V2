using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record HabilityBulletDto(
        string Text,
        string? Badge
    );

    public record HabilityResponse(
        Guid Id,
        string Hability,
        HabilityBulletDto[] Bullets,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateHabilityRequest
    {
        [Required, MaxLength(150)] public string Hability { get; set; } = string.Empty;
        [MinLength(0)] public HabilityBulletDto[] Bullets { get; set; } = Array.Empty<HabilityBulletDto>();
    }

    public class UpdateHabilityRequest : CreateHabilityRequest { }
}
