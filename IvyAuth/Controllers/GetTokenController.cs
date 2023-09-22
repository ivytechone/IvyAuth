using IvyAuth.DataModels;
using IvyAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IvyTech.Logging;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/generatetoken/")]
	public class GenerateTokenController : ControllerBase
	{
		//private readonly ILogger<GenerateTokenController> _logger;
		private readonly IApplicationManager _applicationManager;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IIdentityStore _identityStore;

		public GenerateTokenController(ILogger<GenerateTokenController> logger, ITokenGenerator tokenGenerator, IIdentityStore identityStore, IApplicationManager applicationManager)
		{
			//_logger = logger;
			_tokenGenerator = tokenGenerator;
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

			var app = _applicationManager.IvyAuthApp;

			var token = _tokenGenerator.GenerateToken(identity, app, "default");

			return new OkObjectResult(token);
		}
	}
}