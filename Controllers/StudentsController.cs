using Microsoft.AspNetCore.Mvc;
using StudentService.Models;
using StudentService.Services;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    // 🔹 Créer un étudiant
    [HttpPost] // L'URL sera POST /api/students
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        if (student == null)
            return BadRequest("Données de l'étudiant manquantes");

        var created = await _service.CreateAsync(student);

        if (created == null)
            return BadRequest("Impossible de créer l'étudiant");

        return Ok(created);
    }

    // 🔹 Récupérer tous les étudiants
    [HttpGet] // GET /api/students
    public async Task<IActionResult> GetAll()
    {
        var students = await _service.GetAllAsync();
        return Ok(students);
    }

    // 🔹 Upload CV pour un étudiant
    [HttpPost("upload-cv/{studentId}")] // POST /api/students/upload-cv/{studentId}
    public async Task<IActionResult> UploadCv(
        string studentId,
        [FromForm] IFormFile cvFile)
    {
        if (cvFile == null || cvFile.Length == 0)
            return BadRequest("Aucun fichier PDF sélectionné");

        if (!cvFile.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Seuls les fichiers PDF sont acceptés");

        var result = await _service.UploadCvAsync(studentId, cvFile);

        if (result == null)
            return NotFound("Étudiant non trouvé");

        return Ok(new { message = "CV uploadé avec succès" });
    }

    // 🔹 Télécharger CV
    [HttpGet("download-cv/{studentId}")] // GET /api/students/download-cv/{studentId}
    public async Task<IActionResult> DownloadCv(string studentId)
    {
        var fileBytes = await _service.GetCvAsync(studentId);

        if (fileBytes == null)
            return NotFound("CV non trouvé");

        return File(fileBytes, "application/pdf", "cv.pdf");
    }

    // 🔹 Supprimer CV
    [HttpDelete("delete-cv/{studentId}")] // DELETE /api/students/delete-cv/{studentId}
    public async Task<IActionResult> DeleteCv(string studentId)
    {
        var deleted = await _service.DeleteCvAsync(studentId);

        if (!deleted)
            return NotFound("Étudiant non trouvé");

        return Ok(new { message = "CV supprimé avec succès" });
    }
}