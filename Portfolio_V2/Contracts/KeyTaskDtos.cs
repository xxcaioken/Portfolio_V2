using System.ComponentModel.DataAnnotations;

namespace Portfolio_V2.Contracts
{
    public record KeyTaskTechnologyDto(string Technology, string? TechnologyBadge);

    public record KeyTaskResponse(
        Guid Id,
        string KeyTask,
        string Description,
        KeyTaskTechnologyDto[] Technologies
    );

    public class CreateKeyTaskRequest
    {
        [Required, MaxLength(150)] public string KeyTask { get; set; } = string.Empty;
        [Required, MaxLength(400)] public string Description { get; set; } = string.Empty;
        public KeyTaskTechnologyDto[] Technologies { get; set; } = Array.Empty<KeyTaskTechnologyDto>();
    }

    public class UpdateKeyTaskRequest : CreateKeyTaskRequest { }
}


