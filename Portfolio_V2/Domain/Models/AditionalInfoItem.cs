namespace Portfolio_V2.Domain.Models
{
    public class AditionalInfoItem
    {
        public Guid Id { get; set; }
        public required string AditionalInfo { get; set; }
        public ICollection<AditionalInfoBullet> Bullets { get; set; } = new List<AditionalInfoBullet>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}