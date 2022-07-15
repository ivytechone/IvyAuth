using System.Collections.Specialized;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace IvyTech.DebugLogger
{
	public static class DebugLogger 
	{
		public static Serilog.Core.Logger GetLogger(ConfigurationManager appConfig)
		{
			var config = appConfig.GetSection("DebugLog").Get<DebugLoggerConfig>();

			var elasticSearchOptions = new ElasticsearchSinkOptions(new Uri(config.ElasticSearchURL));
			elasticSearchOptions.AutoRegisterTemplate = false;
			elasticSearchOptions.TypeName = null;
			elasticSearchOptions.InlineFields = true;
			elasticSearchOptions.BatchAction = ElasticOpType.Create;
			elasticSearchOptions.IndexFormat = $"debug-log-{config.Environment}-index-{{0:yyyy.MM}}";
			elasticSearchOptions.ModifyConnectionSettings = c => c.GlobalHeaders(new NameValueCollection { { "Authorization", $"ApiKey {config.ApiKey}" } });

			return new LoggerConfiguration()
						.WriteTo.Elasticsearch(elasticSearchOptions)
						.Enrich.FromLogContext()
						.Enrich.WithProperty("appName", config.AppName)
						.CreateLogger();
		}
	}
}
