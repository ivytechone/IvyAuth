namespace IvyAuth
{
	public class RequestLogger
	{
		private readonly RequestDelegate _requestDelegate;
		private readonly ILogger<RequestLogger> _logger;

		public RequestLogger(RequestDelegate requestDelegate, ILogger<RequestLogger> logger)
		{
			_requestDelegate = requestDelegate;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var requestLogContext = new RequestLogContext();
			context.Items["requestLogContext"] = requestLogContext;

			using (Serilog.Context.LogContext.Push(requestLogContext))
			{
				try
				{
					await _requestDelegate(context);
				}
				catch (Exception ex)
				{
					requestLogContext.Diag = ex.Message;
					_logger.LogError(ex, "Unhandled Exception");
					context.Response.StatusCode = 500;
				}
				finally
				{
					_logger.LogInformation(0x01, "Request {url} {responseCode} {Authenticated}",
						context.Request.Path.Value,
						context.Response.StatusCode,
						requestLogContext.Identity == null ? false : true);
				}
			}
		}
	}
}
