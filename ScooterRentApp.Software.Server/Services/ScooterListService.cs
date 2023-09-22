namespace ScooterRentApp.Software.Server.Services
{
    public class ScooterListService
    {
        private readonly ILogger<ScooterListService> logger;

        public List<ScooterRequest> Scooters { get; set; }

        public ScooterListService(ILogger<ScooterListService> _logger)
        {
            Scooters = new List<ScooterRequest>();
            //Scooters.Add(new ScooterRequest() { Mac = "qwe1", BatteryLevel = "asd", Position = "zxc", RentalTime = "200", Speed = "24" });
            //Scooters.Add(new ScooterRequest() { Mac = "qwe2", BatteryLevel = "asd", Position = "zxc", RentalTime = "0", Speed = "24" });
            this.logger = _logger;
        }

        public void Add(ScooterRequest sc)
        {
            logger.LogInformation($"Scooter {sc.Mac} добавлен в список");
            Scooters.Add(sc);
        }

        public void Update(string mac, string property, string val)
        {
            //logger.LogInformation($"Обновление свойства");
            var f = Scooters.First(x => x.Mac == mac);
            if (f != null)
            {
                switch (property)
                {
                    case "mac":
                        f.Mac = val;
                        break;
                    case "position":
                        f.Position = val;
                        break;
                    case "batterylevel":
                        f.BatteryLevel = val;
                        break;
                    case "speed":
                        f.Speed = val;
                        break;
                    case "rentaltime":
                        f.RentalTime = val;
                        break;
                }
            }
        }
    }
}
