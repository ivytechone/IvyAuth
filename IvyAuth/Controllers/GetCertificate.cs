using System.Security.Cryptography;
using IvyAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/getcertificate/")]
	public class GetCertificateController : ControllerBase
	{
		private readonly ILogger<GenerateTokenController> _logger;
		private readonly ICertificateManager _certificateManager;

		public GetCertificateController(ILogger<GenerateTokenController> logger, ICertificateManager certificateManager)
		{
			logger.LogInformation("Initializing GetCertificate Controller");
			_logger = logger;
			_certificateManager = certificateManager;
		}

		[HttpGet]
		public IActionResult Get()
		{
			using (_logger.BeginScope("GetCertifiate"))
			{
				try
				{
					var cert = _certificateManager.GetPublicKeyCertificate().GetRawCertData();

                    var pem = PemEncoding.Write("CERTIFICATE", cert);
					return new OkObjectResult(new String(pem));
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