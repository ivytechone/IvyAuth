using Serilog.Core;
using Serilog.Events;

namespace IvyTech.Logging
{
	public class RequestLoggerContextEnricher : ILogEventEnricher
	{
        private readonly RequestLoggerContext? requestLoggerContext;

        public RequestLoggerContextEnricher() : this(new HttpContextAccessor())
        {
        }

		public RequestLoggerContextEnricher(IHttpContextAccessor httpContextAccessor)
		{
            requestLoggerContext = httpContextAccessor.HttpContext?.GetRequestLoggerContext();
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
            if (requestLoggerContext is not null)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("requestId", requestLoggerContext.RequestId));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("diag", requestLoggerContext.Diag));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("identity", requestLoggerContext.Identity));
            }
		}
	}
}
