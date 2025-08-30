using System.Text.Json.Serialization.Metadata;
using KoTeSisaApi.Data;
using KoTeSisaApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// JSON: omogući reflection resolver (izbjegava NotSupportedException)
builder.Services.ConfigureHttpJsonOptions(o =>
{
    o.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
});

// EF Core (PostgreSQL)
builder.Services.AddDbContext<AppDb>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// CORS (dozvoli sve za lokalni dev i Swagger)
builder.Services.AddCors(p =>
    p.AddDefaultPolicy(pol => pol.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Swagger (fiksiran server na http://localhost:5029)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KoTeSisa API", Version = "v1" });
    c.AddServer(new OpenApiServer { Url = "http://localhost:5029" });
});

var app = builder.Build();

// Bez HTTPS redirekcije (radimo na http://localhost:5029)
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KoTeSisa API v1");
    });
}

app.MapGet("/healthz", () => "ok");

// POST /saloons
app.MapPost("/saloons", async (AppDb db, Saloon s) =>
{
    db.Saloons.Add(s);
    await db.SaveChangesAsync();
    return Results.Created($"/saloons/{s.SaloonId}", s);
});

app.Run();
