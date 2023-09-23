using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScooterRent.Hardware.HAL;
using ScooterRent.Software.Server.Models;
using ScooterRentApp.Software.Server.Services;
using System.Security.Claims;
using System.Text;

namespace ScooterRentApp.Software.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ScooterController : StartController
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
        public IEnumerable<ScooterDTO> GetAll()
        {
            if (RolesScooterAuth.Any(x => x == "Manager"))
            {
                return ScooterService.Scooters.Select(x=> new ScooterDTO()
                {
                     Speed = x.Speed,
                     BatteryLevel = x.BatteryLevel,
                     Position = x.Position,
                     RentalTime = x.RentalTime,
                     MAC = BitConverter.ToString(x.MAC.GetAddressBytes()).Replace("-", "")
                });
            }
            else
            {
                return ScooterService.Scooters.Where(x => x.RentalTime != 0).Select(x => new ScooterDTO()
                {
                    Speed = x.Speed,
                    BatteryLevel = x.BatteryLevel,
                    Position = x.Position,
                    RentalTime = x.RentalTime,
                    MAC = BitConverter.ToString(x.MAC.GetAddressBytes()).Replace("-", "")
                });
            }
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

        [HttpGet("secured")]
        public IActionResult Secured()
        {
            // Получаем текущего пользователя из контекста
            ClaimsPrincipal user = HttpContext.User;
            StringBuilder SB = new StringBuilder();
            // Получаем и выводим кляймы пользователя
            foreach (Claim claim in user.Claims)
            {
                SB.AppendLine($"{claim.Type}***{claim.Value}");
                // Обрабатываем кляймы, как необходимо
            }
            return Ok(SB.ToString());
        }
    }
}