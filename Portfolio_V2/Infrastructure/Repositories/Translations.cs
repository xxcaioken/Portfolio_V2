using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models.Translations;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class ExperienceTranslationRepository(AppDbContext db) : IExperienceTranslationRepository
    {
        private readonly AppDbContext _db = db;
        public Task<ExperienceItemTranslation?> GetByExperienceIdAsync(Guid experienceId) =>
            _db.ExperienceTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.ExperienceItemId == experienceId);
        public async Task UpsertAsync(ExperienceItemTranslation tr)
        {
            var existing = await _db.ExperienceTranslations.FirstOrDefaultAsync(x => x.ExperienceItemId == tr.ExperienceItemId);
            if (existing is null) _db.ExperienceTranslations.Add(tr); else { existing.Company = tr.Company; existing.Role = tr.Role; existing.Bullets = tr.Bullets; existing.UpdatedAt = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
        }
    }

    public class HabilityTranslationRepository(AppDbContext db) : IHabilityTranslationRepository
    {
        private readonly AppDbContext _db = db;
        public Task<HabilityItemTranslation?> GetByHabilityIdAsync(Guid habilityId) =>
            _db.HabilityTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.HabilityItemId == habilityId);
        public async Task UpsertAsync(HabilityItemTranslation tr)
        {
            var existing = await _db.HabilityTranslations.FirstOrDefaultAsync(x => x.HabilityItemId == tr.HabilityItemId);
            if (existing is null) _db.HabilityTranslations.Add(tr); else { existing.Hability = tr.Hability; existing.UpdatedAt = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
        }
        public async Task<HabilityBulletTranslation[]> GetBulletTranslationsAsync(Guid[] bulletIds) =>
            await _db.HabilityBulletTranslations.AsNoTracking().Where(x => bulletIds.Contains(x.HabilityBulletId)).ToArrayAsync();
        public async Task UpsertBulletAsync(HabilityBulletTranslation tr)
        {
            var existing = await _db.HabilityBulletTranslations.FirstOrDefaultAsync(x => x.HabilityBulletId == tr.HabilityBulletId);
            if (existing is null) _db.HabilityBulletTranslations.Add(tr); else existing.Text = tr.Text;
            await _db.SaveChangesAsync();
        }
    }

    public class AditionalInfoTranslationRepository(AppDbContext db) : IAditionalInfoTranslationRepository
    {
        private readonly AppDbContext _db = db;
        public Task<AditionalInfoItemTranslation?> GetByAditionalInfoIdAsync(Guid aditionalInfoId) =>
            _db.AditionalInfoTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.AditionalInfoItemId == aditionalInfoId);
        public async Task UpsertAsync(AditionalInfoItemTranslation tr)
        {
            var existing = await _db.AditionalInfoTranslations.FirstOrDefaultAsync(x => x.AditionalInfoItemId == tr.AditionalInfoItemId);
            if (existing is null) _db.AditionalInfoTranslations.Add(tr); else { existing.AditionalInfo = tr.AditionalInfo; existing.UpdatedAt = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
        }
        public async Task<AditionalInfoBulletTranslation[]> GetBulletTranslationsAsync(Guid[] bulletIds) =>
            await _db.AditionalInfoBulletTranslations.AsNoTracking().Where(x => bulletIds.Contains(x.AditionalInfoBulletId)).ToArrayAsync();
        public async Task UpsertBulletAsync(AditionalInfoBulletTranslation tr)
        {
            var existing = await _db.AditionalInfoBulletTranslations.FirstOrDefaultAsync(x => x.AditionalInfoBulletId == tr.AditionalInfoBulletId);
            if (existing is null) _db.AditionalInfoBulletTranslations.Add(tr); else existing.Text = tr.Text;
            await _db.SaveChangesAsync();
        }
    }

    public class KeyTaskTranslationRepository(AppDbContext db) : IKeyTaskTranslationRepository
    {
        private readonly AppDbContext _db = db;
        public Task<KeyTaskTranslation?> GetByKeyTaskIdAsync(Guid keyTaskId) =>
            _db.KeyTaskTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.KeyTaskBulletId == keyTaskId);
        public async Task UpsertAsync(KeyTaskTranslation tr)
        {
            var existing = await _db.KeyTaskTranslations.FirstOrDefaultAsync(x => x.KeyTaskBulletId == tr.KeyTaskBulletId);
            if (existing is null) _db.KeyTaskTranslations.Add(tr); else { existing.KeyTask = tr.KeyTask; existing.Description = tr.Description; existing.UpdatedAt = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
        }
    }

    public class AboutTranslationRepository(AppDbContext db) : IAboutTranslationRepository
    {
        private readonly AppDbContext _db = db;
        public Task<AboutInfoTranslation?> GetByAboutIdAsync(Guid aboutId) =>
            _db.AboutTranslations.AsNoTracking().FirstOrDefaultAsync(x => x.AboutInfoId == aboutId);
        public async Task UpsertAsync(AboutInfoTranslation tr)
        {
            var existing = await _db.AboutTranslations.FirstOrDefaultAsync(x => x.AboutInfoId == tr.AboutInfoId);
            if (existing is null) _db.AboutTranslations.Add(tr); else { existing.Name = tr.Name; existing.Title = tr.Title; existing.Summary = tr.Summary; existing.Location = tr.Location; existing.FooterNote = tr.FooterNote; existing.UpdatedAt = DateTime.UtcNow; }
            await _db.SaveChangesAsync();
        }
        public async Task<SocialLinkTranslation[]> GetSocialTranslationsAsync(Guid aboutTrId) =>
            await _db.SocialLinkTranslationsT.AsNoTracking().Where(x => x.AboutInfoTranslationId == aboutTrId).ToArrayAsync();
        public async Task UpsertSocialAsync(SocialLinkTranslation tr)
        {
            var existing = await _db.SocialLinkTranslationsT.FirstOrDefaultAsync(x => x.AboutInfoTranslationId == tr.AboutInfoTranslationId && x.Url == tr.Url);
            if (existing is null) _db.SocialLinkTranslationsT.Add(tr); else { existing.Label = tr.Label; existing.IconKey = tr.IconKey; }
            await _db.SaveChangesAsync();
        }
    }
}


