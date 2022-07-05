using IvyAuth.DataModels;
using IvyAuth.Interfaces;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/generatetoken/")]
	public class GenerateTokenController : ControllerBase
	{
		private readonly ILogger<GenerateTokenController> _logger;
		private readonly ICertificateManager _certificateManager;
		private readonly IApplicationManager _applicationManager;
		private readonly IIdentityStore _identityStore;

		public GenerateTokenController(ILogger<GenerateTokenController> logger, ICertificateManager certificateManager, IIdentityStore identityStore, IApplicationManager applicationManager)
		{
			logger.LogInformation("Initializing TokenController");
			_logger = logger;
			_certificateManager = certificateManager;
			_applicationManager = applicationManager;
			_identityStore = identityStore;
		}

		[HttpPost]
		public IActionResult Post([FromBody] UserNamePassword creds)
		{
			using (_logger.BeginScope("GenerateToken"))
			{
				try
				{
					if (creds == null)
					{
						_logger.LogWarning("Failed to parse input");
						return new UnauthorizedResult();
					}

					var identity = _identityStore.Authenticate(creds);

					if (identity == null)
					{
						_logger.LogWarning("Failed authentication for {username}", creds.UserName);
						return new UnauthorizedResult();
					}

					var cert = _certificateManager.GetCertificateWithPrivateKey();
					var app = _applicationManager.IvyAuthApp;

					var token = JwtBuilder.Create()
					  .WithAlgorithm(new RS256Algorithm(cert))
					  .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds())
					  .AddClaim("iss", "ivytech.one")
					  .AddClaim("aud", app.Id)
					  .AddClaim("sub", identity.Id)
					  .AddClaim("name", identity.Name)
					  .AddClaim("scopes", "")
					  .AddClaim("zoneinfo", identity.TimeZone)
					  .Encode();

					_logger.LogInformation("Successful authentication");
					return new OkObjectResult(token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unexpected Exception");
					return StatusCode(500);
				}
			}
		}
	}
}