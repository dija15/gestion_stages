namespace StudentService.Models
{
    public class SearchStudentDto
    {
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? Diplome { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class StudentDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Diplome { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = [];  // ✅ Simplifié
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];       // ✅ Simplifié
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
