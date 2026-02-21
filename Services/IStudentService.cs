using StudentService.Models;

namespace StudentService.Services;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync();
    Task<Student> CreateAsync(Student student);           // ✅ AJOUTER
    Task<PagedResult<StudentDto>> SearchAsync(SearchStudentDto filter);
    Task<Student?> UploadCvAsync(string studentId, IFormFile cvFile);
    Task<byte[]?> GetCvAsync(string studentId);
   
    Task<bool> DeleteCvAsync(string studentId);// ✅ AJOUTER
}
