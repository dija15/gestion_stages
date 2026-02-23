using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using StudentService.Models;
using StudentService.Settings;

namespace StudentService.Services;

public class StudentDataService : IStudentService
{

    private readonly IMongoCollection<Student> _students;

    private readonly IMongoCollection<Skill> _skills;


    public StudentDataService(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _students = database.GetCollection<Student>(settings.StudentsCollectionName);
        _skills = database.GetCollection<Skill>(settings.SkillsCollectionName);
        
    }

    // ===============================
    // 🔹 GET ALL
    // ===============================
    public async Task<List<Student>> GetAllAsync() =>
        await _students.Find(_ => true).ToListAsync();

    // ===============================
    // 🔹 GET BY ID
    // ===============================
    public async Task<Student?> GetByIdAsync(string id) =>
        await _students.Find(s => s.Id == id).FirstOrDefaultAsync();

    // ===============================
    // 🔹 CREATE
    // ===============================
    public async Task<Student> CreateAsync(Student student)
    {
        student.Id = ObjectId.GenerateNewId().ToString();
        student.CreatedAt = DateTime.UtcNow;

        await _students.InsertOneAsync(student);
        return student;
    }

    // ===============================
    // 🔹 UPDATE
    // ===============================
    public async Task<bool> UpdateAsync(string id, Student student)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) return false;

        student.Id = id; // garder le même id
        await _students.ReplaceOneAsync(s => s.Id == id, student);

        return true;
    }

    // ===============================
    // 🔹 DELETE
    // ===============================
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _students.DeleteOneAsync(s => s.Id == id);
        return result.DeletedCount > 0;
    }

    // ===============================
    // 🔹 UPLOAD CV
    // ===============================
    public async Task<bool> UploadCvAsync(string studentId, IFormFile cvFile)
    {
        var student = await GetByIdAsync(studentId);
        if (student == null) return false;

        using var memoryStream = new MemoryStream();
        await cvFile.CopyToAsync(memoryStream);

        student.CvPdf = memoryStream.ToArray();
        student.CvFileName = cvFile.FileName;

        await _students.ReplaceOneAsync(s => s.Id == studentId, student);
        return true;
    }

    // ===============================
    // 🔹 GET CV
    // ===============================
    public async Task<byte[]?> GetCvAsync(string studentId)
    {
        var student = await GetByIdAsync(studentId);
        return student?.CvPdf;
    }

    // ===============================
    // 🔹 DELETE CV
    // ===============================
    public async Task<bool> DeleteCvAsync(string studentId)
    {
        var student = await GetByIdAsync(studentId);
        if (student == null || student.CvPdf == null)
            return false;

        student.CvPdf = null;
        student.CvFileName = null;

        await _students.ReplaceOneAsync(s => s.Id == studentId, student);
        return true;
    }

    // ===============================
    // 🔹 SEARCH + PAGINATION
    // ===============================
    public async Task<PagedResult<StudentDto>> SearchAsync(SearchStudentDto filter)
    {
        var builder = Builders<Student>.Filter;
        var filters = new List<FilterDefinition<Student>>();

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            filters.Add(builder.Regex(s => s.LastName,
                new BsonRegularExpression(filter.LastName, "i")));

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            filters.Add(builder.Regex(s => s.FirstName,
                new BsonRegularExpression(filter.FirstName, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Diplome))
            filters.Add(builder.Regex(s => s.Diplome,
                new BsonRegularExpression(filter.Diplome, "i")));

        var finalFilter = filters.Count > 0
            ? builder.And(filters)
            : builder.Empty;

        var totalCount = await _students.CountDocumentsAsync(finalFilter);

        var studentsPage = await _students
            .Find(finalFilter)
            .SortByDescending(s => s.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();

        var studentDtos = studentsPage.Select(s => new StudentDto
        {
            Id = s.Id,
            LastName = s.LastName,
            FirstName = s.FirstName,
            Email = s.Email,
            Diplome = s.Diplome,
            Skills = s.Skills
        }).ToList();

        return new PagedResult<StudentDto>
        {
            Items = studentDtos,
            TotalCount = (int)totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }
}