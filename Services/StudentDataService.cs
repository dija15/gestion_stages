using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using StudentService.Models;
using StudentService.Settings;

namespace StudentService.Services
{
    public class StudentDataService : IStudentService
    {
        private readonly IMongoCollection<Student> _students;

        // 🔹 Constructeur principal
        public StudentDataService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _students = database.GetCollection<Student>(settings.StudentsCollectionName);
        }

        // 🔹 Récupérer tous les étudiants
        public async Task<List<Student>> GetAllAsync() =>
            await _students.Find(_ => true).ToListAsync();

        // 🔹 Créer un étudiant
        public async Task<Student> CreateAsync(Student student)
        {
            student.Id = ObjectId.GenerateNewId().ToString();
            student.CreatedAt = DateTime.UtcNow;
            await _students.InsertOneAsync(student);
            return student;
        }

        // 🔹 Upload CV
        public async Task<Student?> UploadCvAsync(string studentId, IFormFile cvFile)
        {
            var student = await _students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
            if (student == null) return null;

            using var memoryStream = new MemoryStream();
            await cvFile.CopyToAsync(memoryStream);
            student.CvPdf = memoryStream.ToArray();
            student.CvFileName = cvFile.FileName;

            await _students.ReplaceOneAsync(s => s.Id == studentId, student);
            return student;
        }

        // 🔹 Récupérer le CV
        public async Task<byte[]?> GetCvAsync(string studentId)
        {
            var student = await _students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
            return student?.CvPdf;
        }

        // 🔹 Supprimer le CV
        public async Task<bool> DeleteCvAsync(string studentId)
        {
            var student = await _students.Find(s => s.Id == studentId).FirstOrDefaultAsync();
            if (student == null || student.CvPdf == null) return false;

            student.CvPdf = null;
            student.CvFileName = null;

            await _students.ReplaceOneAsync(s => s.Id == studentId, student);
            return true;
        }

        // 🔹 Recherche avancée avec pagination
        public async Task<PagedResult<StudentDto>> SearchAsync(SearchStudentDto filter)
        {
            var builder = Builders<Student>.Filter;
            var filters = new List<FilterDefinition<Student>>();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                filters.Add(builder.Regex(s => s.Name, new BsonRegularExpression(filter.Name, "i")));

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
                filters.Add(builder.Regex(s => s.FirstName, new BsonRegularExpression(filter.FirstName, "i")));

            if (!string.IsNullOrWhiteSpace(filter.Diplome))
                filters.Add(builder.Regex(s => s.Diplome, new BsonRegularExpression(filter.Diplome, "i")));

            var finalFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;

            // 🔹 Comptage total
            var totalCount = await _students.CountDocumentsAsync(finalFilter);

            // 🔹 Récupération de la page
            var studentsPage = await _students
                .Find(finalFilter)
                .SortByDescending(s => s.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();

            var studentDtos = studentsPage.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
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
}