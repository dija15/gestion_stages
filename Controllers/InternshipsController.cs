using InternshipService.Models;
using InternshipService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternshipService.Controllers;

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
    public async Task<List<Internship>> Get() =>
        await _service.GetAsync();

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Internship>> Get(string id)
    {
        var internship = await _service.GetAsync(id);
        if (internship == null) return NotFound();
        return internship;
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Post(Internship internship)
    {
        await _service.CreateAsync(internship);
        return CreatedAtAction(nameof(Get), new { id = internship.Id }, internship);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Internship internship)
    {
        var existing = await _service.GetAsync(id);
        if (existing == null) return NotFound();

        internship.Id = id;
        await _service.UpdateAsync(id, internship);
        return NoContent();
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetAsync(id);
        if (existing == null) return NotFound();

        await _service.RemoveAsync(id);
        return NoContent();
    }

    // SEARCH + PAGINATION
    [HttpGet("search")]
    public async Task<List<Internship>> Search(
        string? technology,
        string? location,
        string? company,
        string? type,
        int pageNumber = 1,
        int pageSize = 5)
    {
        return await _service.SearchAsync(
            technology, location, company, type, pageNumber, pageSize);
    }
}