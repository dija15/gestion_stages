using MongoDB.Driver;
using Microsoft.Extensions.Options;
using StudentService.Settings;

namespace StudentService.Data
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);
        }
    }
}
