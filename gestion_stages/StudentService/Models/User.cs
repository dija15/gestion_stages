using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;

    // Champs optionnels pour les entreprises
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }
}


