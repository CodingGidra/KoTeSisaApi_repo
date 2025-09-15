using KoTeSisaApi.Exceptions;

namespace KoTeSisaApi.IoC.ExceptionHandling
{
	public static class ExceptionHandlingExtensions
	{
		public static IServiceCollection AddExceptionHandlingExtension(this IServiceCollection services)
		{
			ArgumentNullException.ThrowIfNull(services, nameof(services));

			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetailsExtension();

			return services;
		}
	}
}
