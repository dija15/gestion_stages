namespace StudentService.Models;

public class RegisterDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Role { get; set; } = "Student"; // Default role
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }
}
