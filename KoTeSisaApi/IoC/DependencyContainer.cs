using KoTeSisaApi.IoC.Cors;
using KoTeSisaApi.IoC.Database;
using KoTeSisaApi.IoC.ExceptionHandling;
using KoTeSisaApi.IoC.Http;
using KoTeSisaApi.IoC.Swagger;

namespace KoTeSisaApi.IoC
{
	public static class DependencyContainer
	{
		public static IServiceCollection AddBuilderServices (this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddExceptionHandlingExtension()
				.AddHttpExtension()
				.AddDatabaseExtension(configuration)
				.AddCorsExtension()
				.AddSwaggerExtension();

			return services;
		}
	}
}
