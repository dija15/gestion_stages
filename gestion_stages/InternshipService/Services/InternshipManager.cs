using InternshipService.Models;
using MongoDB.Driver;

namespace InternshipService.Services;

public class InternshipManager
{
    private readonly IMongoCollection<Internship> _collection;

    public InternshipManager(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
        _collection = database.GetCollection<Internship>(
            config["MongoDbSettings:CollectionName"]);
    }

    public async Task<List<Internship>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<Internship?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Internship internship) =>
        await _collection.InsertOneAsync(internship);

    public async Task UpdateAsync(string id, Internship internship) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, internship);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<Internship>> SearchAsync(
        string? technology,
        string? location,
        string? company,
        string? type,
        int pageNumber,
        int pageSize)
    {
        var filter = Builders<Internship>.Filter.Empty;

        if (!string.IsNullOrEmpty(technology))
            filter &= Builders<Internship>.Filter.AnyEq(x => x.RequiredSkills, technology);

        if (!string.IsNullOrEmpty(location))
            filter &= Builders<Internship>.Filter.Eq(x => x.Location, location);

        if (!string.IsNullOrEmpty(company))
            filter &= Builders<Internship>.Filter.Eq(x => x.Company, company);

        if (!string.IsNullOrEmpty(type))
            filter &= Builders<Internship>.Filter.Eq(x => x.Type, type);

        return await _collection
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }
}