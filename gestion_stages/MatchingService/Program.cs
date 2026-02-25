using MatchingService.Services;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Ajouter Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Neo4j Driver
builder.Services.AddSingleton<IDriver>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    return GraphDatabase.Driver(
        config["Neo4j:Uri"],
        AuthTokens.Basic(
            config["Neo4j:Username"],
            config["Neo4j:Password"]
        )
    );
});

builder.Services.AddScoped<Neo4jService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();