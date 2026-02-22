using InternshipService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipService.Services
{
    public class InternshipManager
    {
        private readonly IMongoCollection<Internship> _collection;

        public InternshipManager(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
            _collection = database.GetCollection<Internship>(config["MongoDbSettings:CollectionName"]);
        }

        public async Task<List<Internship>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Internship?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Internship internship) =>
            await _collection.InsertOneAsync(internship);

        public async Task UpdateAsync(string id, Internship internship) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, internship);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}