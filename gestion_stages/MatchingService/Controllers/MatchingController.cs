using MatchingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : ControllerBase
    {
        private readonly Neo4jService _service;

        public MatchingController(Neo4jService service)
        {
            _service = service;
        }

        [HttpPost("student")]
        public async Task<IActionResult> CreateStudent(string id, string name)
        {
            await _service.CreateStudentAsync(id, name);
            return Ok();
        }

        [HttpPost("internship")]
        public async Task<IActionResult> CreateInternship(string id, string title)
        {
            await _service.CreateInternshipAsync(id, title);
            return Ok();
        }

        [HttpPost("student/{id}/skill")]
        public async Task<IActionResult> AddSkillToStudent(string id, string skill)
        {
            await _service.AddSkillToStudent(id, skill);
            return Ok();
        }

        [HttpPost("internship/{id}/skill")]
        public async Task<IActionResult> AddSkillToInternship(string id, string skill)
        {
            await _service.AddSkillToInternship(id, skill);
            return Ok();
        }

        [HttpGet("match/{studentId}")]
        public async Task<IActionResult> Match(string studentId)
        {
            var result = await _service.MatchStudent(studentId);
            return Ok(result);
        }
    }
}