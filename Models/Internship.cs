using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace InternshipService.Models
{
    public class Internship
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // Id nullable pour MongoDB

        public string Title { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public List<string> RequiredSkills { get; set; }
        public string Duration { get; set; }
    }
}