using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class KeyTaskController(IKeyTaskRepository repo) : ControllerBase
    {
        private readonly IKeyTaskRepository _repo = repo;

        [HttpGet("KeyTasks")]
        [AllowAnonymous]
        public async Task<ActionResult<List<KeyTaskResponse>>> List()
        {
            List<KeyTaskBullet> list = await _repo.ListAsync();
            return Ok(list.Select(MapResponse));
        }

        [HttpGet("KeyTasks/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<KeyTaskResponse>> Get(Guid id)
        {
            KeyTaskBullet? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            return Ok(MapResponse(e));
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
            return CreatedAtAction(nameof(Get), new { id = e.Id }, MapResponse(e));
        }

        [HttpPut("management/KeyTasks/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateKeyTaskRequest req)
        {
            KeyTaskBullet? e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
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

        private static KeyTaskResponse MapResponse(KeyTaskBullet e) => new(
            e.Id,
            e.KeyTask,
            e.description,
            e.Technologies.Select(t => new KeyTaskTechnologyDto(t.Technology, t.TechnologyBadge)).ToArray()
        );
    }
}


