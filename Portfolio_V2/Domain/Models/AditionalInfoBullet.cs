namespace Portfolio_V2.Domain.Models
{
    public class AditionalInfoBullet
    {
        public Guid Id { get; set; }
        public Guid AditionalInfoItemId { get; set; }
        public required string Text { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Level { get; set; }

        public AditionalInfoItem? AditionalInfoItem { get; set; }
    }
}


