using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using System.Reflection.Emit;
using Portfolio_V2.BLL;
using System.Linq;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class AditionalInfoController(IAditionalInfoRepository repo, IAditionalInfoBll bll, IAditionalInfoTranslationRepository trRepo) : ControllerBase
    {
        private readonly IAditionalInfoRepository _repo = repo;
        private readonly IAditionalInfoBll _bll = bll;
        private readonly IAditionalInfoTranslationRepository _trRepo = trRepo;

        [HttpGet("AditionalInfos")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AditionalInfoResponse>>> List()
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var list = await _bll.ListAsync(lang);
            return Ok(list);
        }

        [HttpGet("AditionalInfos/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<AditionalInfoResponse>> Get(Guid id)
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var dto = await _bll.GetAsync(id, lang);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost("management/AditionalInfos")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<AditionalInfoResponse>> Create([FromBody] CreateAditionalInfoRequest req)
        {
            AditionalInfoItem e = GetE(req);
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, await _bll.GetAsync(e.Id, Language.FromHeaderOrQuery(Request)));
        }

        private static AditionalInfoItem GetE(CreateAditionalInfoRequest req)
        {
            var item = new AditionalInfoItem
            {
                AditionalInfo = req.AditionalInfo.Trim(),
                Bullets = new List<AditionalInfoBullet>(),
            };
            foreach (var b in req.Bullets ?? Array.Empty<AditionalInfoBulletDto>())
            {
                item.Bullets.Add(new AditionalInfoBullet
                {
                    Text = b.Text.Trim(),
                    Level = string.IsNullOrWhiteSpace(b.Level) ? null : b.Level.Trim(),
                    StartDate = ParseDateOrNull(b.StartDate),
                    EndDate = ParseDateOrNull(b.EndDate),
                });
            }
            return item;
        }

        [HttpPut("management/AditionalInfos/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAditionalInfoRequest req)
        {
            AditionalInfoItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            string lang = Language.FromHeaderOrQuery(Request);
            if (lang == Language.English)
            {
                var tr = new Domain.Models.Translations.AditionalInfoItemTranslation
                {
                    AditionalInfoItemId = e.Id,
                    AditionalInfo = req.AditionalInfo.Trim(),
                    UpdatedAt = DateTime.UtcNow
                };
                await _trRepo.UpsertAsync(tr);
                // Translate bullets by bullet id when provided
                var baseMap = e.Bullets.ToDictionary(b => b.Id);
                foreach (var reqB in req.Bullets ?? Array.Empty<AditionalInfoBulletDto>())
                {
                    if (reqB.Id is Guid bid && baseMap.ContainsKey(bid))
                    {
                        var btr = new Domain.Models.Translations.AditionalInfoBulletTranslation
                        {
                            AditionalInfoBulletId = bid,
                            Text = reqB.Text.Trim(),
                        };
                        await _trRepo.UpsertBulletAsync(btr);
                    }
                }
                return NoContent();
            }

            e.AditionalInfo = req.AditionalInfo.Trim();
            // Upsert bullets preserving IDs
            var existing = e.Bullets.ToDictionary(b => b.Id);
            var incoming = req.Bullets ?? Array.Empty<AditionalInfoBulletDto>();
            var toKeep = new HashSet<Guid>();
            foreach (var b in incoming)
            {
                if (b.Id is Guid bid && existing.TryGetValue(bid, out var eb))
                {
                    eb.Text = b.Text.Trim();
                    eb.Level = string.IsNullOrWhiteSpace(b.Level) ? null : b.Level.Trim();
                    eb.StartDate = ParseDateOrNull(b.StartDate);
                    eb.EndDate = ParseDateOrNull(b.EndDate);
                    toKeep.Add(bid);
                }
                else
                {
                    e.Bullets.Add(new AditionalInfoBullet
                    {
                        Text = b.Text.Trim(),
                        Level = string.IsNullOrWhiteSpace(b.Level) ? null : b.Level.Trim(),
                        StartDate = ParseDateOrNull(b.StartDate),
                        EndDate = ParseDateOrNull(b.EndDate),
                    });
                }
            }
            var toRemove = e.Bullets.Where(x => !toKeep.Contains(x.Id)).ToList();
            foreach (var rm in toRemove)
            {
                e.Bullets.Remove(rm);
            }
            e.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/AditionalInfos/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            AditionalInfoItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        private static AditionalInfoResponse MapResponse(AditionalInfoItem e) => throw new NotImplementedException();

        private static DateOnly? ParseDateOrNull(string? iso)
        {
            if (string.IsNullOrWhiteSpace(iso)) return null;
            return DateOnly.TryParse(iso, out var d) ? d : null;
        }
    }
}
