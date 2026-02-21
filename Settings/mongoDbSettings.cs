namespace StudentService.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string StudentsCollectionName { get; set; }
    }
}