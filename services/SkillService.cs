using StudentService.Models;
using StudentService.Repositories;

namespace StudentService.Services
{
    public class SkillService
    {
        private readonly SkillRepository _repo;

        public SkillService(SkillRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Skill>> GetAll() => _repo.GetAllAsync();
        public Task<Skill?> GetById(string id) => _repo.GetByIdAsync(id);
        public Task Create(Skill skill) => _repo.CreateAsync(skill);
        public Task Update(string id, Skill skill) => _repo.UpdateAsync(id, skill);
        public Task Delete(string id) => _repo.DeleteAsync(id);
    }
}
