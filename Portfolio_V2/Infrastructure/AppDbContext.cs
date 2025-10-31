using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
        public DbSet<User> Users => Set<User>();
        public DbSet<ExperienceItem> Experiences => Set<ExperienceItem>();
        public DbSet<HabilityItem> Habilitys => Set<HabilityItem>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(entity =>
			{
				entity.ToTable("users");
				entity.HasKey(u => u.Id);
				entity.Property(u => u.Id).HasColumnName("id");
				entity.Property(u => u.Username).HasMaxLength(100).IsRequired().HasColumnName("username").HasConversion<string>();
				entity.Property(u => u.PasswordHash).IsRequired().HasColumnName("password_hash").HasConversion<string>();
				entity.Property(u => u.PasswordSalt).IsRequired().HasColumnName("password_salt").HasConversion<string>();
				entity.Property(u => u.Role).IsRequired().HasColumnName("role").HasConversion<string>();
				entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasConversion<DateTime>();
				entity.HasIndex(u => u.Username).IsUnique();
			});

            modelBuilder.Entity<ExperienceItem>(entity =>
            {
                entity.ToTable("experiences");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Company).IsRequired().HasMaxLength(150).HasColumnName("company");
                entity.Property(e => e.Role).IsRequired().HasMaxLength(120).HasColumnName("role");
                entity.Property(e => e.StartDate).HasColumnName("start_date").HasColumnType("date");
                entity.Property(e => e.EndDate).HasColumnName("end_date").HasColumnType("date");
                entity.Property(e => e.Bullets).HasColumnName("bullets").HasColumnType("text[]");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
		
		    modelBuilder.Entity<HabilityItem>(entity =>
            {
                entity.ToTable("habilitys");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Hability).IsRequired().HasMaxLength(150).HasColumnName("hability");
                entity.Property(e => e.Bullets).HasColumnName("bullets").HasColumnType("text[]");
                entity.Property(e => e.Badge).IsRequired().HasMaxLength(120).HasColumnName("badge");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
		}
	}
}
