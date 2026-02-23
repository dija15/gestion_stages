using StudentService.Models;

namespace StudentService.Services;

public interface IStudentService
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(string id);
    Task<Student> CreateAsync(Student student);
    Task<bool> UpdateAsync(string id, Student student);
    Task<bool> DeleteAsync(string id);

    Task<PagedResult<StudentDto>> SearchAsync(SearchStudentDto filter);

    Task<bool> UploadCvAsync(string studentId, IFormFile cvFile);
    Task<byte[]?> GetCvAsync(string studentId);
    Task<bool> DeleteCvAsync(string studentId);
}