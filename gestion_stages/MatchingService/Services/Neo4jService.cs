using Neo4j.Driver;

namespace MatchingService.Services
{
    public class Neo4jService : IAsyncDisposable
    {
        private readonly IDriver _driver;

        public Neo4jService(IConfiguration config)
        {
            var uri = config["Neo4j:Uri"];
            var user = config["Neo4j:User"];
            var password = config["Neo4j:Password"];

            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task CreateStudentAsync(string studentId, string name)
        {
            var query = @"
                MERGE (s:Student {id: $id})
                SET s.name = $name";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { id = studentId, name });
        }

        public async Task CreateInternshipAsync(string internshipId, string title)
        {
            var query = @"
                MERGE (i:Internship {id: $id})
                SET i.title = $title";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { id = internshipId, title });
        }

        public async Task AddSkillToStudent(string studentId, string skill)
        {
            var query = @"
                MATCH (s:Student {id: $studentId})
                MERGE (sk:Skill {name: $skill})
                MERGE (s)-[:HAS_SKILL]->(sk)";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { studentId, skill });
        }

        public async Task AddSkillToInternship(string internshipId, string skill)
        {
            var query = @"
                MATCH (i:Internship {id: $internshipId})
                MERGE (sk:Skill {name: $skill})
                MERGE (i)-[:REQUIRES]->(sk)";

            await using var session = _driver.AsyncSession();
            await session.RunAsync(query, new { internshipId, skill });
        }

        public async Task<List<string>> MatchStudent(string studentId)
        {
            var query = @"
                MATCH (s:Student {id: $studentId})-[:HAS_SKILL]->(sk)<-[:REQUIRES]-(i:Internship)
                RETURN DISTINCT i.title AS title";

            await using var session = _driver.AsyncSession();
            var result = await session.RunAsync(query, new { studentId });

            var internships = new List<string>();

            await result.ForEachAsync(record =>
            {
                internships.Add(record["title"].As<string>());
            });

            return internships;
        }

        public async ValueTask DisposeAsync()
        {
            await _driver.DisposeAsync();
        }
    }
}