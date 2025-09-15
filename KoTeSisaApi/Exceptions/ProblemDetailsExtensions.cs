using Microsoft.AspNetCore.Diagnostics;

namespace KoTeSisaApi.Exceptions
{
	public static class ProblemDetailsExtensions
	{
		public static IServiceCollection AddProblemDetailsExtension(this IServiceCollection services)
		{
			services.AddProblemDetails(options =>
				options.CustomizeProblemDetails = context =>
				{
					SetInstance(context);
					SetStatusCode(context);
					AddCustomExtensions(context);
				});

			return services;
		}

		private static void SetInstance(ProblemDetailsContext context)
		{
			context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
		}

		private static void SetStatusCode(ProblemDetailsContext context)
		{
			context.ProblemDetails.Status = context.HttpContext.Response.StatusCode;
		}

		private static void AddCustomExtensions(ProblemDetailsContext context)
		{
			var exception = context.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

			SetExceptionTitle(context, exception);

			if (exception != null)
			{
				context.ProblemDetails.Extensions.TryAdd("message", exception.Message);
			}

			context.ProblemDetails.Extensions.TryAdd("timestamp", DateTime.UtcNow);
		}

		private static void SetExceptionTitle(ProblemDetailsContext context, Exception? exception)
		{
			context.ProblemDetails.Type = exception != null ?
				exception.GetType().Name :
				"Unknown exception!";
		}
	}
}
