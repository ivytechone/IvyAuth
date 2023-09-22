using Microsoft.AspNetCore.Mvc;
using IvyAuth.DataModels;
using IvyAuth.Interfaces;
using IvyTech.Logging;

namespace IvyAuth.Controllers
{
	[ApiController]
	public class AuthorizeController : ControllerBase
	{
		private readonly IApplicationManager _applicationManager;
		private readonly ITokenGenerator _tokenGenerator;
		private readonly IIdentityStore _identityStore;

        public AuthorizeController(ITokenGenerator tokenGenerator, IIdentityStore identityStore, IApplicationManager applicationManager)
		{
			_tokenGenerator = tokenGenerator;
			_applicationManager = applicationManager;
			_identityStore = identityStore;
		}

        [HttpGet]
        [Route("authorize/")]
        public IActionResult Get([FromQuery] string client_id, [FromQuery] string scopes, [FromQuery] string state, [FromQuery] string code_challenge, [FromQuery] string redirect_uri) 
        {
            if (string.IsNullOrWhiteSpace(client_id) ||
                string.IsNullOrWhiteSpace(scopes) ||
                string.IsNullOrWhiteSpace(state) ||
                string.IsNullOrWhiteSpace(code_challenge) ||
                string.IsNullOrWhiteSpace(redirect_uri))
            {
                return new BadRequestResult();
            }

            var app = _applicationManager.GetAppById(client_id);
            if (app == null)
            {
                return new BadRequestResult();
            }

            // todo robust validation of parameters as these will be put into client side code

            return UserNamePasswordPage.GetContent(app.Id, state, code_challenge, scopes, redirect_uri);
        }

        public class TokenParams
        {
            [FromForm(Name="grant_type")]
            public string? GrantType { get; set; }

            [FromForm(Name="client_id")]
            public string? ClientId { get; set; }

            [FromForm(Name="code")]
            public string? Code { get; set; }

            [FromForm(Name="code_verifier")]
            public string? CodeVerifier { get; set; }
        }

        [HttpPost]
        [Route("token/")]
        public IActionResult Post([FromForm] TokenParams p)
        {
            if (string.IsNullOrWhiteSpace(p.GrantType) ||
                p.GrantType != "authorization_code" ||
                string.IsNullOrWhiteSpace(p.Code) ||
                string.IsNullOrWhiteSpace(p.CodeVerifier))
            {
                return new BadRequestResult();
            }

            var token = _tokenGenerator.ExchangeCode(p.Code, p.CodeVerifier);
            if (String.IsNullOrWhiteSpace(token))
            {
                return new UnauthorizedResult();
            }
            else
            {
                return new OkObjectResult(token);
            }
        }

        public class CodeParams
        {
            [FromForm(Name="client_id")]
            public string? ClientId { get ;set; }

            [FromForm(Name="username")]
            public string? UserName { get; set; }

            [FromForm(Name="password")]
            public string? Password { get; set; }

            [FromForm(Name="scopes")]
            public string? Scopes { get; set; }

            [FromForm(Name="code_challenge")]
            public string? CodeChallenge { get; set; }
        }

        [HttpPost]
        [Route("code/")]
        public IActionResult Post([FromForm] CodeParams p)
        {
            var requestLogContext = HttpContext.GetRequestLoggerContext();

            if (p == null ||
                string.IsNullOrWhiteSpace(p.UserName) ||
                string.IsNullOrWhiteSpace(p.Password) ||
                string.IsNullOrWhiteSpace(p.CodeChallenge) ||
                string.IsNullOrWhiteSpace(p.ClientId) ||
                string.IsNullOrWhiteSpace(p.Scopes))
			{
				requestLogContext.Diag = DiagCodes.BadRequest;
				return new BadRequestResult();
			}

            var app = _applicationManager.GetAppById(p.ClientId);

            if (app == null)
            {
                return new BadRequestResult();
            }

            var identity = _identityStore.Authenticate(new UserNamePassword() { UserName = p.UserName, Password = p.Password});

			if (identity == null)
			{
				requestLogContext.Diag = DiagCodes.AuthFailed;
				return new UnauthorizedResult();
			}

			requestLogContext.Identity = identity.Id;

			var code = _tokenGenerator.GenerateCode(identity, app, p.Scopes, p.CodeChallenge);
            return new OkObjectResult(code);
        }
    }
}