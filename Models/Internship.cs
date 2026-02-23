using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace StudentService.Models
{
    public class Internship
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = null!;

        [BsonElement("company")]
        public string? Company { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("requiredSkills")]
        public List<string>? RequiredSkills { get; set; }

        [BsonElement("location")]
        public string Location { get; set; } = null!;

        [BsonElement("duration")]
        public string Duration { get; set; } = null!;

        [BsonElement("type")]
        public string Type { get; set; } = null!;
    }
}