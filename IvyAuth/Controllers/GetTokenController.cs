using IvyAuth.DataModels;
using IvyAuth.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using IvyTech.Logging;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/generatetoken/")]
	public class GenerateTokenController : ControllerBase
	{
		//private readonly ILogger<GenerateTokenController> _logger;
		private readonly ICertificateManager _certificateManager;
		private readonly IApplicationManager _applicationManager;
		private readonly IIdentityStore _identityStore;

		public GenerateTokenController(ILogger<GenerateTokenController> logger, ICertificateManager certificateManager, IIdentityStore identityStore, IApplicationManager applicationManager)
		{
			//_logger = logger;
			_certificateManager = certificateManager;
			_applicationManager = applicationManager;
			_identityStore = identityStore;
		}

		[HttpPost]
		public IActionResult Post([FromBody] UserNamePassword creds)
		{
			var requestLogContext = HttpContext.GetRequestLoggerContext();

			if (creds == null)
			{
				requestLogContext.Diag = DiagCodes.BadRequest;
				return new UnauthorizedResult();
			}

			var identity = _identityStore.Authenticate(creds);

			if (identity == null)
			{
				requestLogContext.Diag = DiagCodes.AuthFailed;
				return new UnauthorizedResult();
			}

			requestLogContext.Identity = identity.Id;

			var cert = _certificateManager.GetPrimaryCertificateWithPrivateKey();
			var app = _applicationManager.IvyAuthApp;

			var token = JwtBuilder.Create()
				.WithAlgorithm(new RS256Algorithm(cert.Certificate))
				.AddHeader("kid", cert.Kid)
				.AddClaim("exp", DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
				.AddClaim("iss", "ivytech.one")
				.AddClaim("aud", app.Id)
				.AddClaim("sub", identity.Id)
				.AddClaim("name", identity.Name)
				.AddClaim("scopes", "")
				.AddClaim("zoneinfo", identity.TimeZone)
				.Encode();

			return new OkObjectResult(token);
		}
	}
}