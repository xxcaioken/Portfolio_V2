using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record HabilityResponse(
        Guid Id,
        string Hability,
        string[] Bullets,
        string Badge,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateHabilityRequest
    {
        [Required, MaxLength(150)] public string Hability { get; set; } = string.Empty;
        [MinLength(0)] public string[] Bullets { get; set; } = [];
        [Required, MaxLength(120)] public string Badge { get; set; } = string.Empty;
    }

    public class UpdateHabilityRequest : CreateHabilityRequest { }
}
