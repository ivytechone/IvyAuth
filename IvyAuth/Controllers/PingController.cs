using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/ping/")]
	public class PingController : ControllerBase
	{
		private readonly ILogger<GenerateTokenController> _logger;

		public PingController(ILogger<GenerateTokenController> logger)
		{
			logger.LogInformation("Initializing PingController");
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			_logger.LogInformation("Ping");
			return new OkObjectResult($"IvyAuth {AppInfo.Version}");
		}
	}
}