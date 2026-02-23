using MongoDB.Driver;
using StudentService.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentService.Services
{
    public class InternshipManager
    {
        private readonly IMongoCollection<Internship> _collection;

        public InternshipManager(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDbSettings:DatabaseName"]);
            _collection = database.GetCollection<Internship>(config["MongoDbSettings:InternshipsCollectionName"]);
        }

        // Récupérer tous les stages
        public async Task<List<Internship>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        // Récupérer un stage par ID
        public async Task<Internship?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Créer un nouveau stage
        public async Task CreateAsync(Internship internship) =>
            await _collection.InsertOneAsync(internship);

        // Mettre à jour un stage existant
        public async Task UpdateAsync(string id, Internship internship)
        {
            var filter = Builders<Internship>.Filter.Eq(x => x.Id, id);

            // Ne jamais modifier l'Id
            var update = Builders<Internship>.Update
                .Set(x => x.Title, internship.Title)
                .Set(x => x.Company, internship.Company)
                .Set(x => x.Description, internship.Description)
                .Set(x => x.RequiredSkills, internship.RequiredSkills ?? new List<string>())
                .Set(x => x.Location, internship.Location)
                .Set(x => x.Duration, internship.Duration)
                .Set(x => x.Type, internship.Type);

            var result = await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Aucun stage trouvé avec l'id {id}");
            }
        }

        // Supprimer un stage
        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}