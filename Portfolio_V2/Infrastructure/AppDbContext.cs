using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
        public DbSet<User> Users => Set<User>();
        public DbSet<ExperienceItem> Experiences => Set<ExperienceItem>();
        public DbSet<HabilityItem> Habilitys => Set<HabilityItem>();
        public DbSet<HabilityBullet> HabilityBullets => Set<HabilityBullet>();
        public DbSet<AditionalInfoItem> AditionalInfos => Set<AditionalInfoItem>();
        public DbSet<AditionalInfoBullet> AditionalInfoBullets => Set<AditionalInfoBullet>();
        public DbSet<KeyTaskTechnology> KeyTaskTechnologies => Set<KeyTaskTechnology>();

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
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.HasMany(e => e.Bullets)
                      .WithOne(b => b.HabilityItem!)
                      .HasForeignKey(b => b.HabilityItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HabilityBullet>(entity =>
            {
                entity.ToTable("hability_bullets");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).HasColumnName("id");
                entity.Property(b => b.HabilityItemId).HasColumnName("hability_id");
                entity.Property(b => b.Text).IsRequired().HasMaxLength(300).HasColumnName("text");
                entity.Property(b => b.Badge).HasMaxLength(120).HasColumnName("badge");
            });

            modelBuilder.Entity<AditionalInfoItem>(entity =>
            {
                entity.ToTable("aditionalInfos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AditionalInfo).IsRequired().HasMaxLength(150).HasColumnName("aditionalInfo");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.HasMany(e => e.Bullets)
                      .WithOne(b => b.AditionalInfoItem!)
                      .HasForeignKey(b => b.AditionalInfoItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AditionalInfoBullet>(entity =>
            {
                entity.ToTable("aditionalInfo_bullets");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).HasColumnName("id");
                entity.Property(b => b.AditionalInfoItemId).HasColumnName("aditionalInfo_id");
                entity.Property(b => b.Text).IsRequired().HasMaxLength(300).HasColumnName("text");
                entity.Property(b => b.Level).HasMaxLength(120).HasColumnName("level");
                entity.Property(b => b.StartDate).HasColumnName("start_date").HasColumnType("date");
                entity.Property(b => b.EndDate).HasColumnName("end_date").HasColumnType("date");
            });
            
            modelBuilder.Entity<KeyTaskBullet>(entity =>
            {
                entity.ToTable("KeyTask");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).HasColumnName("id");
                entity.Property(b => b.KeyTask).HasColumnName("KeyTask");
                entity.Property(b => b.description).IsRequired().HasMaxLength(400).HasColumnName("description");
                entity.HasMany(b => b.Technologies)
                      .WithOne(t => t.KeyTaskBullet!)
                      .HasForeignKey(t => t.KeyTaskBulletId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<KeyTaskTechnology>(entity =>
            {
                entity.ToTable("keytask_technologies");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnName("id");
                entity.Property(t => t.KeyTaskBulletId).HasColumnName("keytask_id");
                entity.Property(t => t.Technology).IsRequired().HasMaxLength(120).HasColumnName("technology");
                entity.Property(t => t.TechnologyBadge).HasColumnName("technologyBadge");
            });
        }
	}
}
