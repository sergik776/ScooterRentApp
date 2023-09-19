using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScooterRentApp.Software.Server.Services;

namespace ScooterRentApp.Software.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ScooterController : ControllerBase
    {
        private readonly ILogger<ScooterController> _logger;
        private readonly ScooterListService ScooterService;

        public ScooterController(ILogger<ScooterController> logger, ScooterListService scooterService)
        {
            ScooterService = scooterService;
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<ScooterRequest> GetAll()
        {
            return ScooterService.Scooters;
        }
    }
}