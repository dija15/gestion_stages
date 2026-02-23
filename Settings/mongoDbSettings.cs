namespace StudentService.Settings
{
    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string StudentsCollectionName { get; set; }
        public required string SkillsCollectionName { get; set; }
        public required string InternshipsCollectionName { get; set; } // ✅ Ajouté pour les stages
    }
}