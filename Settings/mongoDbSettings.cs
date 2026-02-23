namespace StudentService.Settings
{
    public class MongoDbSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string StudentsCollectionName { get; set; }
<<<<<<< HEAD
        public required string SkillsCollectionName { get; set; }
        public required string InternshipsCollectionName { get; set; } // ✅ Ajouté pour les stages
=======
>>>>>>> 7058157f69f460eeee12acf3ca7192394e281f41
    }
}