using KoTeSisaApi.IoC;
using KoTeSisaApi.IoC.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBuilderServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
	app.UseSwaggerApp();
}

app.UseExceptionHandler();

app.MapControllers();

app.Run();
