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
				var (hash, salt) = AuthService.CreatePasswordHash("admin123");
				var admin = new User
				{
					Id = Guid.NewGuid(),
					Username = "admin",
					PasswordHash = hash,
					PasswordSalt = salt
				};
				db.Users.Add(admin);
				await db.SaveChangesAsync();
			}
		}
	}
}
