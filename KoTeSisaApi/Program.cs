using KoTeSisaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// JSON
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
});

// EF Core (Postgres)
builder.Services.AddDbContext<AppDb>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS
builder.Services.AddCors(p =>
    p.AddDefaultPolicy(pol => pol.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// 🚩 Dodaj controllere
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KoTeSisa API", Version = "v1" });
    c.AddServer(new OpenApiServer { Url = "http://localhost:5029" });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KoTeSisa API v1"));
}

//app.MapGet("/healthz", () => "ok");

// 🚩 Mapiraj controllere
app.MapControllers();

app.Run();
