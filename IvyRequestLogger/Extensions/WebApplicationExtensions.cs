namespace IvyTech.RequestLogger
{
	public static class WebApplicationExtensions
	{
		public static WebApplication UseIvyRequestLogger(this WebApplication app)
		{
			app.UseMiddleware<RequestLogger>(app.Configuration);
			return app;
		}
	}
}
