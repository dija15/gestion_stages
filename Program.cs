using StudentService.Settings;
using StudentService.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurer MongoDbSettings depuis appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

// 🔹 Ajouter les services
builder.Services.AddScoped<IStudentService, StudentDataService>(); // Service étudiant
builder.Services.AddScoped<InternshipManager>(); // Service stage

// 🔹 Ajouter les contrôleurs et Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Config Swagger seulement en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();