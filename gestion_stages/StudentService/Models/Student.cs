using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StudentService.Models
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Diplome { get; set; } = string.Empty;

        // ✅ Collection simplifiée
        public List<string> Skills { get; set; } = [];

        // 🔥 PDF
        public byte[]? CvPdf { get; set; }
        public string? CvFileName { get; set; }

        // ✅ Constructeur principal
        public Student()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; set; }
    }
}
