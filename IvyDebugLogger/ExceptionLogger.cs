namespace IvyTech.DebugLogger
{
	public class ExceptionLogger 
	{
		private readonly RequestDelegate _requestDelegate;
		private readonly ILogger _logger;

		public ExceptionLogger(RequestDelegate requestDelegate, ILogger logger)
		{
			_logger = logger;
			_requestDelegate = requestDelegate;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _requestDelegate(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled Exception");
				throw;
			}			
		}
	}
}
