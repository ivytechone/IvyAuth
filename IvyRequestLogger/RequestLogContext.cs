using Serilog.Core;
using Serilog.Events;

namespace IvyTech.RequestLogger
{
	public class RequestLoggerContext : ILogEventEnricher
	{
		public RequestLoggerContext()
		{
			RequestId = Guid.NewGuid().ToString().ToUpperInvariant();
		}

		public string RequestId { get; }
		public string? Diag { get; set; }
		public string? Identity { get; set; }

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("requestId", RequestId));
			logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("diag", Diag));
			logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("identity", Identity));
		}
	}
}
