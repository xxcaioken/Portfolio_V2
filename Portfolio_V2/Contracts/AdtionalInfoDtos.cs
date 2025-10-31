using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record AditionalInfoResponse(
        Guid Id,
        string AditionalInfo,
        string[] Bullets,
        DateTime? StartDate,
        DateTime? EndDate,
        string? Level,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );

    public class CreateAditionalInfoRequest
    {
        [Required, MaxLength(150)] public required string AditionalInfo { get; set; }
        [MinLength(0)] public string[] Bullets { get; set; } = [];
        public DateTime? StartDate { get; set; } = null; 
        public DateTime? EndDate { get; set; } = null;
        public string? Level { get; set; } = null;
    }

    public class UpdateAditionalInfoRequest : CreateAditionalInfoRequest { }
}


