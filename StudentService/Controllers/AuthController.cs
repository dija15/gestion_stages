using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using StudentService.Models;
using StudentService.Services;
using BCrypt.Net;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMongoCollection<User> _users;
    private readonly JWTService _jwtService;

    public AuthController(IMongoDatabase database, JWTService jwtService)
    {
        _users = database.GetCollection<User>("Users");
        _jwtService = jwtService;
    }

    // ===============================
    // REGISTER STUDENT / ADMIN
    // ===============================
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
        if (existingUser != null)
            return BadRequest("User already exists.");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Name = dto.Name,
            Role = dto.Role,
            CompanyName = dto.CompanyName,
            Phone = dto.Phone
        };

        await _users.InsertOneAsync(user);

        return Ok(new { Message = $"{dto.Role} registered successfully" });
    }
    // ===============================
    // LOGIN (Tous les rôles)
    // ===============================
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _jwtService.GenerateToken(user.Email, user.Role);

        return Ok(new
        {
            Token = token,
            Role = user.Role
        });
    }
}
