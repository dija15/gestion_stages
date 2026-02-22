using Microsoft.AspNetCore.Mvc;
using StudentService.Models;
using StudentService.Services;

namespace StudentService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentController(IStudentService service)
    {
        _service = service;
    }

    // ===============================
    // 🔹 GET ALL STUDENTS
    // ===============================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _service.GetAllAsync();
        return Ok(students);
    }

    // ===============================
    // 🔹 GET STUDENT BY ID
    // ===============================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var student = await _service.GetByIdAsync(id);

        if (student == null)
            return NotFound("Student not found");

        return Ok(student);
    }

    // ===============================
    // 🔹 CREATE STUDENT
    // ===============================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        if (student == null)
            return BadRequest("Student data is required");

        var created = await _service.CreateAsync(student);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // ===============================
    // 🔹 UPDATE STUDENT
    // ===============================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Student student)
    {
        var updated = await _service.UpdateAsync(id, student);

        if (!updated)
            return NotFound("Student not found");

        return NoContent();
    }

    // ===============================
    // 🔹 DELETE STUDENT
    // ===============================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("Student not found");

        return NoContent();
    }

    // ===============================
    // 🔹 UPLOAD CV
    // ===============================
    [HttpPost("upload-cv/{studentId}")]
    public async Task<IActionResult> UploadCv(string studentId, [FromForm] IFormFile cvFile)
    {
        if (cvFile == null || cvFile.Length == 0)
            return BadRequest("No file selected");

        if (!cvFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only PDF files are allowed");

        var result = await _service.UploadCvAsync(studentId, cvFile);

        if (!result)
            return NotFound("Student not found");

        return Ok(new { message = "CV uploaded successfully" });
    }

    // ===============================
    // 🔹 DOWNLOAD CV
    // ===============================
    [HttpGet("download-cv/{studentId}")]
    public async Task<IActionResult> DownloadCv(string studentId)
    {
        var fileBytes = await _service.GetCvAsync(studentId);

        if (fileBytes == null)
            return NotFound("CV not found");

        return File(fileBytes, "application/pdf", "cv.pdf");
    }

    // ===============================
    // 🔹 DELETE CV
    // ===============================
    [HttpDelete("delete-cv/{studentId}")]
    public async Task<IActionResult> DeleteCv(string studentId)
    {
        var deleted = await _service.DeleteCvAsync(studentId);

        if (!deleted)
            return NotFound("Student not found");

        return Ok(new { message = "CV deleted successfully" });
    }
}