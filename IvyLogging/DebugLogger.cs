using System.Collections.Specialized;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace IvyTech.Logging
{
	public static class DebugLogger 
	{
		private static Serilog.ILogger? _logger;

		public static Serilog.ILogger? Logger
		{
			get => _logger;
		}

		public static Serilog.ILogger? CreateLogger(ConfigurationManager appConfig)
		{
			if (_logger == null)
			{
				var config = appConfig.GetSection("IvyLogging").Get<IvyLoggingConfig>();

				if (config is null ||
					config.ElasticSearchURL is null ||
					config.ApiKey is null ||
					config.Environment is null ||
					config.AppName is null)
				{
					return null;
				}

				var elasticSearchOptions = new ElasticsearchSinkOptions(new Uri(config.ElasticSearchURL));
				elasticSearchOptions.AutoRegisterTemplate = false;
				elasticSearchOptions.TypeName = null; 
				elasticSearchOptions.InlineFields = true;
				elasticSearchOptions.BatchAction = ElasticOpType.Create;
				elasticSearchOptions.IndexFormat = $"debug-log-{config.Environment}-index-{{0:yyyy.MM}}";
				elasticSearchOptions.ModifyConnectionSettings = c => c.GlobalHeaders(new NameValueCollection { { "Authorization", $"ApiKey {config.ApiKey}" } });

				_logger = new LoggerConfiguration()
							.WriteTo.Elasticsearch(elasticSearchOptions)
							.Enrich.FromLogContext()
							.Enrich.WithProperty("appName", config.AppName)
							.CreateLogger();
			}

			return _logger;
		}
	}
}
