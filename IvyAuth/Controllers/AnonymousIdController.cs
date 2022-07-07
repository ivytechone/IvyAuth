using IvyAuth.DataModels;
using IvyAuth.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
    [ApiController]
	public class AnonymousIdController : ControllerBase
	{
		private readonly ILogger<GenerateTokenController> _logger;
		private readonly ICertificateManager _certificateManager;
		private readonly IApplicationManager _applicationManager;

        public AnonymousIdController(Logger<GenerateTokenController> logger, ICertificateManager certificateManager, IApplicationManager applicationManager)
        {
            _logger = logger;
            _certificateManager = certificateManager;
            _applicationManager = applicationManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var cert = _certificateManager.GetAidCertificateWithPrivateKey();
			var app = _applicationManager.IvyAuthApp;

            var token = JwtBuilder.Create()
                .WithAlgorithm(new RS256Algorithm(cert))
                .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
                .AddClaim("iss", "ivytech.one")
                .AddClaim("aud", _applicationManager.BuildNumberApp)
                .AddClaim("scopes", "")
                .AddClaim("zoneinfo", "")
                .Encode();

            return new OkObjectResult(token);
        }

    }
}