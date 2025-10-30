using System;

namespace Portfolio_V2.Domain.Models
{
    public class ExperienceItem
    {
        public Guid Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string[] Bullets { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}


