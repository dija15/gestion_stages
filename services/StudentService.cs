using StudentService.Models;
using StudentService.Repositories;

namespace StudentService.Services;

public class StudentLogicService
{
    private readonly StudentRepository _repository;

    public StudentLogicService(StudentRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Student>> GetAll() => _repository.GetAllAsync();
    public Task<Student?> GetById(string id) => _repository.GetByIdAsync(id);
    public Task Create(Student student) => _repository.CreateAsync(student);
    public Task Update(string id, Student student) => _repository.UpdateAsync(id, student);
    public Task Delete(string id) => _repository.DeleteAsync(id);
}
