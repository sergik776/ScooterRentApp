using Microsoft.AspNetCore.Mvc;
using ScooterRentApp.Software.Server.Services;

namespace ScooterRentApp.Software.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ScooterListService ScooterService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ScooterListService scooterService)
        {
            ScooterService = scooterService;
            _logger = logger;
        }

        [HttpGet(Name = "GetScooters")]
        public IEnumerable<ScooterRequest> GetScooters()
        {
            return ScooterService.Scooters;
        }
    }
}