namespace IvyTech.RequestLogger
{
	public static class HttpContextExtensions
	{
		public static RequestLoggerContext GetRequestLoggerContext(this HttpContext httpContext)
		{
			if (httpContext.Items.TryGetValue(Constants.RequestLoggerContextKey, out object? requestLoggerContext) && requestLoggerContext is RequestLoggerContext)
			{
				return (RequestLoggerContext)requestLoggerContext;
			}
			throw new MissingRequestLoggerContextException();
		}
	}
}
