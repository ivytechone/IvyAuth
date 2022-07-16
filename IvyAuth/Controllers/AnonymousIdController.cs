using IvyAuth.Extensions;
using IvyAuth.Interfaces;
using IvyTech.Logging;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
    [ApiController]
	public class AnonymousIdController : ControllerBase
	{
		private readonly ILogger<AnonymousIdController> _logger;
		private readonly ICertificateManager _certificateManager;
		private readonly IApplicationManager _applicationManager;

        public AnonymousIdController(ILogger<AnonymousIdController> logger, ICertificateManager certificateManager, IApplicationManager applicationManager)
        {
            _logger = logger;
            _certificateManager = certificateManager;
            _applicationManager = applicationManager;
        }

        [HttpGet]
        [Route("api/anonymousid/")]
        public IActionResult Get()
        {
            const string apiName = "GetAnonymousId";
            var appId = HttpContext.GetApplicationId();
            var timeZone = HttpContext.GetTimeZone();
            var app = _applicationManager.GetAppById(appId);

            if (app is null)
            {
                _logger.LogWarning("{apiname}: {msg} {appid}", apiName, "x-application-id invalid or missing", appId);
                this.HttpContext.GetRequestLoggerContext().Diag = "invalidAppId";
                return new BadRequestObjectResult("x-application-id invalid or missing");
            }

            // TimeZone is optional default to UTC
            if (timeZone is null)
            {
                _logger.LogInformation("{apiname}: {msg}", apiName, "Using default timezone");
                timeZone = "Etc/UTC";
            }
            // Check if user provided Windows Format, Convert and use IANA format
            else if (TimeZoneInfo.TryConvertWindowsIdToIanaId(timeZone, out string? ianaId))
            {
                timeZone = ianaId;
            }
            // Check if user provided valid IANA timezone
            else if (!TimeZoneInfo.TryConvertIanaIdToWindowsId(timeZone, out string? windowsId))
            {
                _logger.LogWarning("{apiname}: {msg}", apiName, "x-timezone contains invalid time zone");
                this.HttpContext.GetRequestLoggerContext().Diag = "invalidTimeZone";
                return new BadRequestObjectResult("x-timezone contains invalid time zone");
            }

            var cert = _certificateManager.GetAidCertificateWithPrivateKey();
            var identity = Guid.NewGuid().ToString();

            var token = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(cert))
                .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .AddClaim("iss", "ivytech.one")
                .AddClaim("sub", identity)
                .AddClaim("aud", app.Id)
                .AddClaim("zoneinfo", timeZone)
                .Encode();

            this.HttpContext.GetRequestLoggerContext().Identity = identity;

            _logger.LogInformation("{apiname}: Success {timezone} {app}", apiName, timeZone, app.Id);
            return new OkObjectResult(token);
        }
    }
}