using StudentService.Services;
using StudentService.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurer MongoDbSettings depuis appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

// 🔹 Ajouter le service StudentDataService avec IStudentService
builder.Services.AddScoped<IStudentService>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new StudentDataService(settings);
});

// 🔹 Ajouter les contrôleurs
builder.Services.AddControllers();

// 🔹 Swagger (optionnel)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();