namespace Portfolio_V2.Domain.Models
{
    public class KeyTaskTechnology
    {
        public Guid Id { get; set; }
        public Guid KeyTaskBulletId { get; set; }
        public string Technology { get; set; } = string.Empty;
        public string? TechnologyBadge { get; set; }

        public KeyTaskBullet? KeyTaskBullet { get; set; }
    }
}


