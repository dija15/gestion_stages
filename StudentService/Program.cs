using MongoDB.Driver;
using StudentService.Models;
using StudentService.Services;
using StudentService.Settings;
using StudentService.Repositories;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB Settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register MongoDB Client and Database
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

// Register Repositories
builder.Services.AddScoped<SkillRepository>();
builder.Services.AddScoped<StudentRepository>();

// Register Services
builder.Services.AddScoped<IStudentService, StudentDataService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddSingleton<JWTService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<FileUploadOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();