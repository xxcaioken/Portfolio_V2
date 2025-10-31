namespace Portfolio_V2.Domain.Models
{
    public class AditionalInfoItem
    {
        public Guid Id { get; set; }
        public required string AditionalInfo { get; set; }
        public string[] Bullets { get; set; } = [];
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public string? Level { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}