using Portfolio_V2.Application.Services;
using Portfolio_V2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_V2.Infrastructure.Seed
{
	public static class DatabaseSeeder
	{
		public static async Task SeedAsync(AppDbContext db)
		{
			if (!await db.Users.AnyAsync())
			{
				(string hash, string salt) = AuthService.CreatePasswordHash("admin123");
				User admin = new()
				{
					Id = Guid.NewGuid(),
					Username = "admin",
					PasswordHash = hash,
					PasswordSalt = salt,
					Role = Role.Admin
				};
				db.Users.Add(admin);
				await db.SaveChangesAsync();
			}
		}
	}
}
