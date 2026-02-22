using InternshipService.Models;
using InternshipService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternshipsController : ControllerBase
    {
        private readonly InternshipManager _service;

        public InternshipsController(InternshipManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<Internship>> Get() => await _service.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Internship>> Get(string id)
        {
            var internship = await _service.GetAsync(id);
            if (internship == null) return NotFound();
            return internship;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Internship internship)
        {
            await _service.CreateAsync(internship);
            return CreatedAtAction(nameof(Get), new { id = internship.Id }, internship);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Internship internship)
        {
            await _service.UpdateAsync(id, internship);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}