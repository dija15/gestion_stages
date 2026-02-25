using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InternshipService.Models;

public class Internship
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> RequiredSkills { get; set; } = new();
    public string Location { get; set; } = null!;
    public string Company { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public string Type { get; set; } = null!; // PFE, Stage d’été
    public string Status { get; set; } = "Open"; // Open, Closed, Expired
    public DateTime Deadline { get; set; }
}