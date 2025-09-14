using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace KoTeSisaApi.IoC.Swagger
{
	public static class SwaggerExtensions
	{
		public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "KoTeSisa API", Version = "v1" });
				c.AddServer(new OpenApiServer { Url = "http://localhost:5029" });

				c.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
				{
					Type = "string",
					Format = "date",
					Example = new OpenApiString("2025-09-02")
				});
				c.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema
				{
					Type = "string",
					Format = "time",
					Example = new OpenApiString("11:20:00")
				});
			});

			return services;
		}

		public static void UseSwaggerApp(this WebApplication app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KoTeSisa API v1"));
		}
	}
}
