using MongoDB.Driver;
using StudentService.Models;
using StudentService.Data;

namespace StudentService.Repositories
{
    public class SkillRepository
    {
        private readonly IMongoCollection<Skill> _skills;

        public SkillRepository(MongoDbContext context)
        {
            _skills = context.Database.GetCollection<Skill>("Skills");
        }

        public async Task<List<Skill>> GetAllAsync() =>
            await _skills.Find(_ => true).ToListAsync();

        public async Task<Skill?> GetByIdAsync(string id) =>
            await _skills.Find(s => s.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Skill skill) =>
            await _skills.InsertOneAsync(skill);

        public async Task UpdateAsync(string id, Skill skill) =>
            await _skills.ReplaceOneAsync(s => s.Id == id, skill);

        public async Task DeleteAsync(string id) =>
            await _skills.DeleteOneAsync(s => s.Id == id);
    }
}
