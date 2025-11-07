using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio_V2.BLL;
using Portfolio_V2.Contracts;
using Portfolio_V2.Domain.Models;
using Portfolio_V2.Infrastructure.Repositories;

namespace Portfolio_V2.Controllers
{
    [ApiController]
    [Route("")]
    public class TestimonialsController(ITestimonialRepository repo, ITestimonialBll bll, ITestimonialTranslationRepository trRepo) : ControllerBase
    {
        private readonly ITestimonialRepository _repo = repo;
        private readonly ITestimonialBll _bll = bll;
        private readonly ITestimonialTranslationRepository _trRepo = trRepo;

        [HttpGet("Testimonials")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TestimonialResponse>>> List()
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var list = await _bll.ListAsync(lang);
            return Ok(list);
        }

        [HttpGet("Testimonials/{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<TestimonialResponse>> Get(Guid id)
        {
            string lang = Language.FromHeaderOrQuery(Request);
            var dto = await _bll.GetAsync(id, lang);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost("management/Testimonials")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<TestimonialResponse>> Create([FromBody] CreateTestimonialRequest req)
        {
            var e = new Testimonial
            {
                Name = req.Name.Trim(),
                Highlight = req.Highlight.Trim(),
            };
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = e.Id }, await _bll.GetAsync(e.Id, Language.FromHeaderOrQuery(Request))!);
        }

        [HttpPut("management/Testimonials/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTestimonialRequest req)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();

            string lang = Language.FromHeaderOrQuery(Request);
            if (lang == Language.English)
            {
                var tr = new Domain.Models.Translations.TestimonialTranslation
                {
                    TestimonialId = e.Id,
                    Name = string.IsNullOrWhiteSpace(req.Name) ? null : req.Name.Trim(),
                    Highlight = string.IsNullOrWhiteSpace(req.Highlight) ? null : req.Highlight.Trim(),
                    UpdatedAt = DateTime.UtcNow
                };
                await _trRepo.UpsertAsync(tr);
                return NoContent();
            }

            e.Name = req.Name.Trim();
            e.Highlight = req.Highlight.Trim();
            e.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("management/Testimonials/{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var e = await _repo.GetAsync(id);
            if (e is null) return NotFound();
            await _repo.DeleteAsync(e);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
    }
}



