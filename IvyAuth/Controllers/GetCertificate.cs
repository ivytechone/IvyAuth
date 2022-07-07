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
					var certs= _certificateManager.GetPublicKeyCertificates();
                    var certsData = certs.Select(x => x.GetRawCertData());
                    var pemsData = certsData.Select(x => new String(PemEncoding.Write("CERTIFICATE", x)));
                    return new OkObjectResult(pemsData);
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