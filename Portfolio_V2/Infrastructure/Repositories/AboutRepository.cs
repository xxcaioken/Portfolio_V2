using Microsoft.EntityFrameworkCore;
using Portfolio_V2.Domain.Models;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public class AboutRepository(AppDbContext db) : IAboutRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<AboutInfo?> GetAsync()
        {
            return await _db.AboutInfos.Include(a => a.Socials).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(AboutInfo about)
        {
            var existing = await _db.AboutInfos.Include(a => a.Socials).FirstOrDefaultAsync();
            if (existing is null)
            {
                await _db.AboutInfos.AddAsync(about);
            }
            else
            {
                existing.Name = about.Name;
                existing.Title = about.Title;
                existing.Summary = about.Summary;
                existing.Location = about.Location;
                existing.Phone = about.Phone;
                existing.Email = about.Email;
                existing.Linkedin = about.Linkedin;
                existing.Github = about.Github;
                existing.AvatarUrl = about.AvatarUrl;
                existing.FooterNote = about.FooterNote;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.Socials.Clear();
                foreach (var s in about.Socials)
                {
                    existing.Socials.Add(new SocialLink { Label = s.Label, Url = s.Url, IconKey = s.IconKey });
                }
            }
            await _db.SaveChangesAsync();
        }
    }
}


