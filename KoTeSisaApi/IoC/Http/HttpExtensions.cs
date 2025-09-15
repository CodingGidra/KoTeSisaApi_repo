using System.Text.Json.Serialization.Metadata;

namespace KoTeSisaApi.IoC.Http
{
	public static class HttpExtensions
	{
		public static IServiceCollection AddHttpExtension(this IServiceCollection services) 
		{
			services.ConfigureHttpJsonOptions(o =>
			{
				o.SerializerOptions.TypeInfoResolverChain.Add(new DefaultJsonTypeInfoResolver());
			});

			return services;
		}
	}
}
