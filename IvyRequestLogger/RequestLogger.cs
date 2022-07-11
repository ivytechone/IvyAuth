using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Collections.Specialized;

namespace IvyTech.RequestLogger
{
	public class RequestLogger 
	{
		private readonly RequestDelegate _requestDelegate;
		private static Serilog.ILogger? _logger;

		public RequestLogger(RequestDelegate requestDelegate, ConfigurationManager configuration)
		{
			if (_logger is null)
			{
				var requestLoggerConfig = configuration.GetSection("RequestLogger").Get<RequestLoggerConfig>();

				if (requestLoggerConfig != null)
				{
					_logger = GetLogger(requestLoggerConfig);
				}
				else
				{
					throw new RequestLoggerConfigException();
				}
			}

			_requestDelegate = requestDelegate;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var requestLoggerContext = new RequestLoggerContext();
			context.Items[Constants.RequestLoggerContextKey] = requestLoggerContext;

			using (Serilog.Context.LogContext.Push(requestLoggerContext))
			{
				try
				{
					await _requestDelegate(context);
				}
				catch (Exception)
				{
					requestLoggerContext.Diag = DiagCodes.UnhandledException;
					context.Response.StatusCode = 500;
					throw;
				}
				finally
				{
					_logger?.Information("{method} {path} {responseCode}",
						context.Request.Method,
						context.Request.Path.Value,
						context.Response.StatusCode);
				}
			}
		}

		public static Serilog.Core.Logger GetLogger(RequestLoggerConfig config)
		{
			var elasticSearchOptions = new ElasticsearchSinkOptions(new Uri(config.ElasticSearchURL));
			elasticSearchOptions.AutoRegisterTemplate = true;
			elasticSearchOptions.AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7;
			elasticSearchOptions.TypeName = null;
			elasticSearchOptions.InlineFields = true;
			elasticSearchOptions.BatchAction = ElasticOpType.Create;
			elasticSearchOptions.IndexFormat = $"RequestLog-{config.Environment}-index-{{0:yyyy.MM}}";
			elasticSearchOptions.ModifyConnectionSettings = c => c.GlobalHeaders(new NameValueCollection { { "Authorization", $"ApiKey {config.ApiKey}" } });

			return new LoggerConfiguration()
						.WriteTo.Elasticsearch(elasticSearchOptions)
						.Enrich.FromLogContext()
						.Enrich.WithProperty("AppName", config.AppName)
						.CreateLogger();
		}
	}
}
