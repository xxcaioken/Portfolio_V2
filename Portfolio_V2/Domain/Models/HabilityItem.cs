using System;

namespace Portfolio_V2.Domain.Models
{
    public class HabilityItem
    {
        public Guid Id { get; set; }
        public string Hability { get; set; } = string.Empty;
        public string[] Bullets { get; set; } = [];
        public string Badge { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}