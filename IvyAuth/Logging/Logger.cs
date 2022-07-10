using IvyAuth.Config;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Collections.Specialized;

namespace IvyAuth
{
	public static class Logger 
	{
		public static Serilog.Core.Logger GetLogger(SerilogConfig config)
		{
			var elasticSearchOptions = new ElasticsearchSinkOptions(new Uri(config.elasticSearchUrl));
			elasticSearchOptions.AutoRegisterTemplate = true;
			elasticSearchOptions.AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7;
			elasticSearchOptions.TypeName = null;
			elasticSearchOptions.BatchAction = ElasticOpType.Create;
			elasticSearchOptions.IndexFormat = $"{config.indexName}-index-{{0:yyyy.MM}}";
			elasticSearchOptions.ModifyConnectionSettings = c => c.GlobalHeaders(new NameValueCollection { { "Authorization", $"ApiKey {config.apiKey}" } });

			return new LoggerConfiguration()
						.WriteTo.Console()
						.WriteTo.Elasticsearch(elasticSearchOptions)
						.MinimumLevel.Error()
						.MinimumLevel.Override("IvyAuth", LogEventLevel.Information)
						.Enrich.FromLogContext()
						.CreateLogger();
		}
	}
}
