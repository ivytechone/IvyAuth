using Microsoft.AspNetCore.Mvc;

namespace IvyAuth.Controllers
{
	[ApiController]
	[Route("api/ping/")]
	public class PingController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return new OkObjectResult($"IvyAuth {AppInfo.Version}");
		}
	}
}