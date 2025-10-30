using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
        public DbSet<User> Users => Set<User>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("users");
				entity.HasKey(u => u.Id);
				entity.Property(u => u.Id).HasColumnName("id");
				entity.Property(u => u.Username).HasMaxLength(100).IsRequired().HasColumnName("username");
				entity.Property(u => u.PasswordHash).IsRequired().HasColumnName("password_hash");
				entity.Property(u => u.PasswordSalt).IsRequired().HasColumnName("password_salt");
				entity.Property(u => u.CreatedAt).HasColumnName("created_at");
				entity.HasIndex(u => u.Username).IsUnique();
			});
		}
	}
}
