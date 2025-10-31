using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;
using Portfolio_V2.BLL;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class KeyTaskController(IKeyTaskRepository repo, IKeyTaskBll bll, IKeyTaskTranslationRepository trRepo) : ControllerBase
    {
        private readonly IKeyTaskRepository _repo = repo;
        private readonly IKeyTaskBll _bll = bll;
        private readonly IKeyTaskTranslationRepository _trRepo = trRepo;

        [HttpGet("KeyTasks")]
        [AllowAnonymous]
        public async Task<ActionResult<List<KeyTaskResponse>>> List()
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var list = await _bll.ListAsync(lang);
            return Ok(list);
        }

        [HttpGet("KeyTasks/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<KeyTaskResponse>> Get(Guid id)
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var dto = await _bll.GetAsync(id, lang);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost("management/KeyTasks")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<KeyTaskResponse>> Create([FromBody] CreateKeyTaskRequest req)
        {
            KeyTaskBullet e = new KeyTaskBullet
            {
                KeyTask = req.KeyTask.Trim(),
                description = req.Description.Trim(),
                Technologies = new List<KeyTaskTechnology>(),
            };
            foreach (var t in req.Technologies)
            {
                e.Technologies.Add(new KeyTaskTechnology
                {
                    Technology = t.Technology.Trim(),
                    TechnologyBadge = string.IsNullOrWhiteSpace(t.TechnologyBadge) ? null : t.TechnologyBadge.Trim(),
                });
            }
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, await _bll.GetAsync(e.Id, Language.FromHeaderOrQuery(Request)));
        }

        [HttpPut("management/KeyTasks/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateKeyTaskRequest req)
        {
            KeyTaskBullet? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            string lang = Language.FromHeaderOrQuery(Request);
            if (lang == Language.English)
            {
                var tr = new Domain.Models.Translations.KeyTaskTranslation
                {
                    KeyTaskBulletId = e.Id,
                    KeyTask = req.KeyTask.Trim(),
                    Description = req.Description.Trim(),
                    UpdatedAt = DateTime.UtcNow
                };
                await _trRepo.UpsertAsync(tr);
                return NoContent();
            }
            e.KeyTask = req.KeyTask.Trim();
            e.description = req.Description.Trim();
            e.Technologies.Clear();
            foreach (var t in req.Technologies)
            {
                e.Technologies.Add(new KeyTaskTechnology
                {
                    Technology = t.Technology.Trim(),
                    TechnologyBadge = string.IsNullOrWhiteSpace(t.TechnologyBadge) ? null : t.TechnologyBadge.Trim(),
                });
            }
            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/KeyTasks/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            KeyTaskBullet? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        private static KeyTaskResponse MapResponse(KeyTaskBullet e) => throw new NotImplementedException();
    }
}


