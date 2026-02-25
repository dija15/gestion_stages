using Microsoft.AspNetCore.Mvc;
using StudentService.Models;
using StudentService.Services;

namespace StudentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly SkillService _service;

        public SkillController(SkillService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id) => Ok(await _service.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Post(Skill skill)
        {
            await _service.Create(skill);
            return Ok(skill);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Skill skill)
        {
            await _service.Update(id, skill);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
