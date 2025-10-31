namespace Portfolio_V2.Domain.Models
{
    public class KeyTaskBullet
    {
        public Guid Id { get; set; }
        public string KeyTask { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public ICollection<KeyTaskTechnology> Technologies { get; set; } = new List<KeyTaskTechnology>();
    }
}


