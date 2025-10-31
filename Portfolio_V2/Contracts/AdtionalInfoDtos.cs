using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record AditionalInfoBulletDto(
        Guid? Id,
        string Text,
        string? Level,
        string? StartDate,
        string? EndDate
    );

    public record AditionalInfoResponse(
        Guid Id,
        string AditionalInfo,
        AditionalInfoBulletDto[] Bullets,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateAditionalInfoRequest
    {
        [Required, MaxLength(150)] public required string AditionalInfo { get; set; }
        [MinLength(0)] public AditionalInfoBulletDto[] Bullets { get; set; } = [];
    }

    public class UpdateAditionalInfoRequest : CreateAditionalInfoRequest { }
}


