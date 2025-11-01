using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Domain.Models.Translations;

namespace Portfolio_V2.Infrastructure
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
        public DbSet<User> Users => Set<User>();
        public DbSet<ExperienceItem> Experiences => Set<ExperienceItem>();
        public DbSet<HabilityItem> Habilities => Set<HabilityItem>();
        public DbSet<HabilityBullet> HabilityBullets => Set<HabilityBullet>();
        public DbSet<AditionalInfoItem> AditionalInfos => Set<AditionalInfoItem>();
        public DbSet<AditionalInfoBullet> AditionalInfoBullets => Set<AditionalInfoBullet>();
        public DbSet<KeyTaskTechnology> KeyTaskTechnologies => Set<KeyTaskTechnology>();
        public DbSet<AboutInfo> AboutInfos => Set<AboutInfo>();
        public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
        public DbSet<RecommendationLetter> RecommendationLetters => Set<RecommendationLetter>();
        // Translations
        public DbSet<ExperienceItemTranslation> ExperienceTranslations => Set<ExperienceItemTranslation>();
        public DbSet<HabilityItemTranslation> HabilityTranslations => Set<HabilityItemTranslation>();
        public DbSet<HabilityBulletTranslation> HabilityBulletTranslations => Set<HabilityBulletTranslation>();
        public DbSet<AditionalInfoItemTranslation> AditionalInfoTranslations => Set<AditionalInfoItemTranslation>();
        public DbSet<AditionalInfoBulletTranslation> AditionalInfoBulletTranslations => Set<AditionalInfoBulletTranslation>();
        public DbSet<KeyTaskTranslation> KeyTaskTranslations => Set<KeyTaskTranslation>();
        public DbSet<AboutInfoTranslation> AboutTranslations => Set<AboutInfoTranslation>();
        public DbSet<SocialLinkTranslation> SocialLinkTranslationsT => Set<SocialLinkTranslation>();

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
                entity.ToTable("Habilities");
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

            modelBuilder.Entity<AboutInfo>(e =>
            {
                e.ToTable("about_info");
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).HasColumnName("id");
                e.Property(a => a.Name).HasMaxLength(150).IsRequired().HasColumnName("name");
                e.Property(a => a.Title).HasMaxLength(150).IsRequired().HasColumnName("title");
                e.Property(a => a.Summary).HasMaxLength(2000).HasColumnName("summary");
                e.Property(a => a.Location).HasMaxLength(150).HasColumnName("location");
                e.Property(a => a.Phone).HasMaxLength(50).HasColumnName("phone");
                e.Property(a => a.Email).HasMaxLength(200).HasColumnName("email");
                e.Property(a => a.Linkedin).HasMaxLength(300).HasColumnName("linkedin");
                e.Property(a => a.Github).HasMaxLength(300).HasColumnName("github");
                e.Property(a => a.AvatarUrl).HasMaxLength(500).HasColumnName("avatar_url");
                e.Property(a => a.FooterNote).HasMaxLength(300).HasColumnName("footer_note");
                e.Property(a => a.CreatedAt).HasColumnName("created_at");
                e.Property(a => a.UpdatedAt).HasColumnName("updated_at");
                e.HasMany(a => a.Socials)
                 .WithOne(s => s.About!)
                 .HasForeignKey(s => s.AboutInfoId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SocialLink>(e =>
            {
                e.ToTable("about_social_links");
                e.HasKey(s => s.Id);
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.AboutInfoId).HasColumnName("about_id");
                e.Property(s => s.Label).HasMaxLength(100).IsRequired().HasColumnName("label");
                e.Property(s => s.Url).HasMaxLength(500).IsRequired().HasColumnName("url");
                e.Property(s => s.IconKey).HasMaxLength(120).HasColumnName("icon_key");
            });

            modelBuilder.Entity<RecommendationLetter>(e =>
            {
                e.ToTable("recommendation_letters");
                e.HasKey(r => r.Id);
                e.Property(r => r.Id).HasColumnName("id");
                e.Property(r => r.ImageUrlPt).HasMaxLength(500).HasColumnName("image_url_pt");
                e.Property(r => r.ImageUrlEn).HasMaxLength(500).HasColumnName("image_url_en");
                e.Property(r => r.CreatedAt).HasColumnName("created_at");
                e.Property(r => r.UpdatedAt).HasColumnName("updated_at");
            });

            // Translations mapping
            modelBuilder.Entity<ExperienceItemTranslation>(e =>
            {
                e.ToTable("experiences_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.ExperienceItemId).HasColumnName("experience_id");
                e.Property(x => x.Company).HasMaxLength(150).HasColumnName("company");
                e.Property(x => x.Role).HasMaxLength(120).HasColumnName("role");
                e.Property(x => x.Bullets).HasColumnName("bullets").HasColumnType("text[]");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<HabilityItemTranslation>(e =>
            {
                e.ToTable("Habilities_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.HabilityItemId).HasColumnName("hability_id");
                e.Property(x => x.Hability).HasMaxLength(150).HasColumnName("hability");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<HabilityBulletTranslation>(e =>
            {
                e.ToTable("hability_bullets_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.HabilityBulletId).HasColumnName("hability_bullet_id");
                e.Property(x => x.Text).HasMaxLength(300).HasColumnName("text");
            });

            modelBuilder.Entity<AditionalInfoItemTranslation>(e =>
            {
                e.ToTable("aditionalInfos_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.AditionalInfoItemId).HasColumnName("aditionalInfo_id");
                e.Property(x => x.AditionalInfo).HasMaxLength(150).HasColumnName("aditionalInfo");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<AditionalInfoBulletTranslation>(e =>
            {
                e.ToTable("aditionalInfo_bullets_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.AditionalInfoBulletId).HasColumnName("aditionalInfo_bullet_id");
                e.Property(x => x.Text).HasMaxLength(300).HasColumnName("text");
            });

            modelBuilder.Entity<KeyTaskTranslation>(e =>
            {
                e.ToTable("keytask_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.KeyTaskBulletId).HasColumnName("keytask_id");
                e.Property(x => x.KeyTask).HasColumnName("keytask");
                e.Property(x => x.Description).HasMaxLength(400).HasColumnName("description");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<AboutInfoTranslation>(e =>
            {
                e.ToTable("about_info_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.AboutInfoId).HasColumnName("about_id");
                e.Property(x => x.Name).HasMaxLength(150).HasColumnName("name");
                e.Property(x => x.Title).HasMaxLength(150).HasColumnName("title");
                e.Property(x => x.Summary).HasMaxLength(2000).HasColumnName("summary");
                e.Property(x => x.Location).HasMaxLength(150).HasColumnName("location");
                e.Property(x => x.FooterNote).HasMaxLength(300).HasColumnName("footer_note");
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<SocialLinkTranslation>(e =>
            {
                e.ToTable("about_social_links_en");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.AboutInfoTranslationId).HasColumnName("about_tr_id");
                e.Property(x => x.Label).HasMaxLength(100).HasColumnName("label");
                e.Property(x => x.Url).HasMaxLength(500).HasColumnName("url");
                e.Property(x => x.IconKey).HasMaxLength(120).HasColumnName("icon_key");
            });
        }
	}
}
