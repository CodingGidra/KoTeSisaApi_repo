using Microsoft.AspNetCore.Diagnostics;

namespace KoTeSisaApi.Exceptions
{
	public sealed class GlobalExceptionHandler(
		IProblemDetailsService problemDetailsService,
		ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
	{

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			// logger

			return false;
		}

	}
}
