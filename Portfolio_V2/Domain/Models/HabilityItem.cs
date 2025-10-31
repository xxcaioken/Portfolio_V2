using System;

namespace Portfolio_V2.Domain.Models
{
    public class HabilityItem
    {
        public Guid Id { get; set; }
        public string Hability { get; set; } = string.Empty;
        public ICollection<HabilityBullet> Bullets { get; set; } = new List<HabilityBullet>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}