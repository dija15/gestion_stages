using MongoDB.Driver;
using StudentService.Models;

namespace StudentService.Data;

public class MongoDbContext
{

    public MongoDbContext(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDb:ConnectionString"]);
        Database = client.GetDatabase(config["MongoDb:DatabaseName"]);
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<Student> Students =>
        Database.GetCollection<Student>("Students");
}
