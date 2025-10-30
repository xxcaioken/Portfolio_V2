using System;

namespace Portfolio_V2.Domain.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string PasswordSalt { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
