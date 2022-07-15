namespace IvyTech.DebugLogger
{
	public static class WebApplicationExtensions
	{
		public static WebApplication UseIvyExceptionLogger(this WebApplication app)
		{
			app.UseMiddleware<ExceptionLogger>();
			return app;
		}
	}
}