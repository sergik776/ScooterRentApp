using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;
using ScooterRentApp.Hardware.Server.HardwareProtocol;

namespace ScooterRentApp.Hardware.Server
{
    public class Scooter : IBaseScooter
    {
        protected TcpClient Client;

        public ushort RentalTime { get; protected set; }

        public Position Position { get; protected set; }

        public byte BatteryLevel { get; protected set; }

        public sbyte Speed { get; protected set; }

        public EndPoint? IP { get { return Client.Client.RemoteEndPoint; } }

        public PhysicalAddress MAC { get; protected set; }

        public event PropertyHandler? PropertyChanged;

        public Scooter(PhysicalAddress mac, TcpClient c)
        {
            Client = c;
            Position = new Position(0, 0);
            MAC = mac;
            start();
        }

        void start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[17];
                    Client.Client.Receive(buffer);

                    try
                    {
                        var pack = PacketDesiarizable.Desiarizable(buffer);
                        SetScooter(pack);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
            });
        }

        private void SetScooter(ScooterDataPacket pack)
        {
            switch (pack.Property)
            {
                case Enums.RecieveProperty.MAC:
                    MAC = (PhysicalAddress)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.MAC);
                    break;

                case RecieveProperty.Position:
                    Position = (Position)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Position);
                    break;

                case RecieveProperty.BateryLevel:
                    BatteryLevel = (byte)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.BateryLevel);
                    break;

                case RecieveProperty.Speed:
                    Speed = (sbyte)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Speed);
                    break;

                case RecieveProperty.RentalTime:
                    RentalTime = (ushort)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.RentalTime);
                    break;

                default:

                    break;
            }
        }

        public override string ToString()
        {
            return $"Scooter: [MAC: {MAC}] [IP: {IP}] [RentalTime: {RentalTime}] [Position: {Position}] [BateryLevel: {BatteryLevel}%] [Speed: {Speed}km/h]";
        }
    }
}
