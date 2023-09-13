using IvyAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IvyAuth.Controllers
{
    [ApiController]
    public class JwtKeysController : ControllerBase
    {
        private readonly ILogger<JwtKeysController> _logger;
        private readonly ICertificateManager _certificateManager;

        public JwtKeysController(ILogger<JwtKeysController> logger, ICertificateManager certificateManager)
        {
            _logger = logger;
            _certificateManager = certificateManager;
        }

        [HttpGet]
        [Route("api/jwtkeys/")]
        public IActionResult GetJwtKeys()
        {
            const string apiName = "GetJwtKeys";
 
            _logger.LogInformation("{apiname}: Success", apiName);
            return new OkObjectResult( JsonConvert.SerializeObject(_certificateManager.GetPublicKeys()));
        }
    }
}