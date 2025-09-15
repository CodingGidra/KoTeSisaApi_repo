using KoTeSisaApi.IoC.Cors;
using KoTeSisaApi.IoC.Database;
using KoTeSisaApi.IoC.Http;
using KoTeSisaApi.IoC.Swagger;

namespace KoTeSisaApi.IoC
{
	public static class DependencyContainer
	{
		public static IServiceCollection AddBuilderServices (this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddHttpExtension()
				.AddDatabaseExtension(configuration)
				.AddCorsExtension()
				.AddSwaggerExtension();

			return services;
		}
	}
}
