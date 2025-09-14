using KoTeSisaApi.Data;
using Microsoft.EntityFrameworkCore;

namespace KoTeSisaApi.IoC.Database
{
	public static class DatabaseExtensions
	{
		public static IServiceCollection AddDatabaseExtension(this IServiceCollection services, IConfiguration configuration) 
		{
			services.AddDbContext<AppDb>(opt =>
				opt.UseNpgsql(configuration.GetConnectionString("Default")));

			return services;
		}
	}
}
