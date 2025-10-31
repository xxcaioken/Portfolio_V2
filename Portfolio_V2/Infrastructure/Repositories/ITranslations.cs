using Portfolio_V2.Domain.Models.Translations;

namespace Portfolio_V2.Infrastructure.Repositories
{
    public interface IExperienceTranslationRepository
    {
        Task<ExperienceItemTranslation?> GetByExperienceIdAsync(Guid experienceId);
        Task UpsertAsync(ExperienceItemTranslation tr);
    }

    public interface IHabilityTranslationRepository
    {
        Task<HabilityItemTranslation?> GetByHabilityIdAsync(Guid habilityId);
        Task UpsertAsync(HabilityItemTranslation tr);
        Task<HabilityBulletTranslation[]> GetBulletTranslationsAsync(Guid[] bulletIds);
        Task UpsertBulletAsync(HabilityBulletTranslation tr);
    }

    public interface IAditionalInfoTranslationRepository
    {
        Task<AditionalInfoItemTranslation?> GetByAditionalInfoIdAsync(Guid aditionalInfoId);
        Task UpsertAsync(AditionalInfoItemTranslation tr);
        Task<AditionalInfoBulletTranslation[]> GetBulletTranslationsAsync(Guid[] bulletIds);
        Task UpsertBulletAsync(AditionalInfoBulletTranslation tr);
    }

    public interface IKeyTaskTranslationRepository
    {
        Task<KeyTaskTranslation?> GetByKeyTaskIdAsync(Guid keyTaskId);
        Task UpsertAsync(KeyTaskTranslation tr);
    }

    public interface IAboutTranslationRepository
    {
        Task<AboutInfoTranslation?> GetByAboutIdAsync(Guid aboutId);
        Task UpsertAsync(AboutInfoTranslation tr);
        Task<SocialLinkTranslation[]> GetSocialTranslationsAsync(Guid aboutTrId);
        Task UpsertSocialAsync(SocialLinkTranslation tr);
    }
}


