using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using Portfolio_V2.BLL;
using System.Linq;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class HabilityController(IHabilityRepository repo, IHabilityBll bll, IHabilityTranslationRepository trRepo) : ControllerBase
    {
        private readonly IHabilityRepository _repo = repo;
        private readonly IHabilityBll _bll = bll;
        private readonly IHabilityTranslationRepository _trRepo = trRepo;

        [HttpGet("Habilities")]
        [AllowAnonymous]
        public async Task<ActionResult<List<HabilityResponse>>> List()
        {
            string lang = BLL.Language.FromHeaderOrQuery(Request);
            var list = await _bll.ListAsync(lang);
            return Ok(list);
        }

        [HttpGet("Habilities/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<HabilityResponse>> Get(Guid id)
        {
            string lang = BLL.Language.FromHeaderOrQuery(Request);
            var dto = await _bll.GetAsync(id, lang);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost("management/Habilities")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<HabilityResponse>> Create([FromBody] CreateHabilityRequest req)
        {   
            HabilityItem e = GetE(req);
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, await _bll.GetAsync(e.Id, BLL.Language.FromHeaderOrQuery(Request))!);
        }

        private static HabilityItem GetE(CreateHabilityRequest req)
        {
            var item = new HabilityItem
            {
                Hability = req.Hability.Trim(),
                Bullets = new List<HabilityBullet>(),
            };
            foreach (var b in req.Bullets ?? Array.Empty<HabilityBulletDto>())
            {
                item.Bullets.Add(new HabilityBullet
                {
                    Text = b.Text.Trim(),
                    Badge = string.IsNullOrWhiteSpace(b.Badge) ? null : b.Badge.Trim(),
                });
            }
            return item;
        }

        [HttpPut("management/Habilities/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabilityRequest req)
        {
            HabilityItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            string lang = BLL.Language.FromHeaderOrQuery(Request);
            if (lang == BLL.Language.English)
            {
                var tr = new Domain.Models.Translations.HabilityItemTranslation
                {
                    HabilityItemId = e.Id,
                    Hability = req.Hability.Trim(),
                    UpdatedAt = DateTime.UtcNow
                };
                await _trRepo.UpsertAsync(tr);
                // Translate bullets by bullet id when provided
                var baseMap = e.Bullets.ToDictionary(b => b.Id);
                foreach (var reqB in req.Bullets ?? Array.Empty<HabilityBulletDto>())
                {
                    if (reqB.Id is Guid bid && baseMap.ContainsKey(bid))
                    {
                        var btr = new Domain.Models.Translations.HabilityBulletTranslation
                        {
                            HabilityBulletId = bid,
                            Text = reqB.Text.Trim(),
                        };
                        await _trRepo.UpsertBulletAsync(btr);
                    }
                }
                return NoContent();
            }

            e.Hability = req.Hability.Trim();
            // Upsert bullets preserving IDs
            var existing = e.Bullets.ToDictionary(b => b.Id);
            var incoming = req.Bullets ?? Array.Empty<HabilityBulletDto>();
            var toKeep = new HashSet<Guid>();
            foreach (var b in incoming)
            {
                if (b.Id is Guid bid && existing.TryGetValue(bid, out var eb))
                {
                    eb.Text = b.Text.Trim();
                    eb.Badge = string.IsNullOrWhiteSpace(b.Badge) ? null : b.Badge.Trim();
                    toKeep.Add(bid);
                }
                else
                {
                    e.Bullets.Add(new HabilityBullet
                    {
                        Text = b.Text.Trim(),
                        Badge = string.IsNullOrWhiteSpace(b.Badge) ? null : b.Badge.Trim(),
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

        [HttpDelete("management/Habilities/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            HabilityItem? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        private static HabilityResponse MapResponse(HabilityItem e) => throw new NotImplementedException();
    }
}
