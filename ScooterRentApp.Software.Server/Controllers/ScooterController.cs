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
        private readonly SendCommandService _sendCommandService;

        public ScooterController(ILogger<ScooterController> logger, ScooterListService scooterService, SendCommandService sendCommandService)
        {
            _sendCommandService = sendCommandService;
            ScooterService = scooterService;
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<ScooterRequest> GetAll()
        {
            return ScooterService.Scooters;
        }

        [HttpPost]
        public IActionResult SetRentalTime(Models.SetRentalTime model)
        {
            byte[] byteArray = new byte[model.Mac.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(model.Mac.Substring(2 * i, 2), 16);
            }
            _sendCommandService.SendRentalTime(byteArray, model.Seconds);
            return Ok();
        }
    }
}