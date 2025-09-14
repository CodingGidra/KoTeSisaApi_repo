namespace KoTeSisaApi.IoC.Cors
{
	public static class CorsExtensions
	{
		public static IServiceCollection AddCorsExtension(this IServiceCollection services) 
		{ 
			services.AddCors(p =>
				p.AddDefaultPolicy(pol => pol.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

			return services;
		}
	}
}
