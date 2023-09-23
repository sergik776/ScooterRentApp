using ScooterRent.Hardware.HAL;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.NetworkInformation;

namespace ScooterRent.Software.Server.Models
{
    public class ServerScooter : IBaseScooter
    {
        public Position Position { get; set; }

        public byte BatteryLevel { get; set; }

        public sbyte Speed { get; set; }

        public ushort RentalTime { get; set; }

        public EndPoint? IP { get; set; }

        public PhysicalAddress MAC { get; set; }

        public event PropertyHandler? PropertyChanged;

        public ServerScooter(byte[] mac) 
        {
            MAC = new PhysicalAddress(mac);
            Position = new Position(0, 0);
            BatteryLevel = 0;
            Speed = 0;
            RentalTime = 0;
            IP = null;
        }
    }

    public class ScooterDTO
    {
        public string MAC { get; set; }
        public Position Position { get; set; }
        public byte BatteryLevel { get; set; }
        public sbyte Speed { get; set; }
        public ushort RentalTime { get; set; }
    }
}
