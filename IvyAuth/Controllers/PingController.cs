using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/ping/")]
	public class PingController : ControllerBase
	{
		private readonly ILogger<PingController> _logger;

		public PingController(ILogger<PingController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Get()
		{
			_logger.LogInformation("Ping Called");
			return new OkObjectResult($"IvyAuth {AppInfo.Version}");
		}
	}
}