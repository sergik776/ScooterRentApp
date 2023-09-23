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
using Google.Protobuf;

namespace ScooterRentApp.Hardware.Server
{
    public class Scooter : IScooterManager
    {
        protected TcpClient Client;

        public ushort RentalTime { get; protected set; }

        public Position Position { get; protected set; }

        public byte BatteryLevel { get; protected set; }

        public sbyte Speed { get; protected set; }

        public EndPoint? IP { get { return Client.Client.RemoteEndPoint; } }

        public PhysicalAddress MAC { get; protected set; }

        public event PropertyHandler? PropertyChanged;

        private ScooterGRPCService.ScooterGRPCServiceClient GRPClient;

        public Scooter(PhysicalAddress mac, TcpClient c, ScooterGRPCService.ScooterGRPCServiceClient gRPClient)
        {
            Client = c;
            Position = new Position(0, 0);
            MAC = mac;
            start();
            GRPClient = gRPClient;

            GRPClient.AddScooter(new MacRequest
            {
                Mac = ByteString.CopyFrom(MAC.GetAddressBytes())
            });
        }

        void start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[17];
                    Client.Client.Receive(buffer);
                    var pack = PacketDesiarizable.Desiarizable(buffer);
                    SetScooter(pack);
                }
            });
        }

        private void SetScooter(ScooterDataPacket pack)
        {
            ScooterResponse response = null;
            switch (pack.Property)
            {
                case Enums.RecieveProperty.MAC:
                    MAC = (PhysicalAddress)pack.Value;
                    response = GRPClient.AddScooter(new MacRequest
                    {
                        Mac = ByteString.CopyFrom(MAC.GetAddressBytes())
                    });
                    PropertyChanged?.Invoke(MAC, RecieveProperty.MAC);
                    break;

                case RecieveProperty.Position:
                    Position = (Position)pack.Value;
                    response = GRPClient.ChangePosition(new PositionRequest() 
                    {
                        Mac = ByteString.CopyFrom(MAC.GetAddressBytes()),
                        Latitude = Position.Latitude,
                        Longitude = Position.Longitude
                    });
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Position);
                    break;

                case RecieveProperty.BateryLevel:
                    BatteryLevel = (byte)pack.Value;
                    response = GRPClient.ChangeBatteryLevel(new BatteryLevelRequest() 
                    {
                        Mac = ByteString.CopyFrom(MAC.GetAddressBytes()),
                        BatteryLevel = BatteryLevel
                    });
                    PropertyChanged?.Invoke(MAC, RecieveProperty.BateryLevel);
                    break;

                case RecieveProperty.Speed:
                    Speed = (sbyte)pack.Value;
                    response = GRPClient.ChangeSpeed(new SpeedRequest()
                    {
                        Mac = ByteString.CopyFrom(MAC.GetAddressBytes()),
                        Speed = Speed
                    });
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Speed);
                    break;

                case RecieveProperty.RentalTime:
                    RentalTime = (ushort)pack.Value;
                    response = GRPClient.ChangeRentalTime(new RentalTimeRequest()
                    {
                        Mac = ByteString.CopyFrom(MAC.GetAddressBytes()),
                        RentalTime = RentalTime
                    });
                    PropertyChanged?.Invoke(MAC, RecieveProperty.RentalTime);
                    break;
                default: break;
            }
        }

        public override string ToString()
        {
            var q = TimeSpan.FromSeconds(RentalTime);
            return $"Scooter: [MAC: {MAC}] [IP: {IP}] [RentalTime: {q.Hours}:{q.Minutes}:{q.Seconds}] [Position: {Position}] [BateryLevel: {BatteryLevel}%] [Speed: {Speed}km/h]";
        }

        public void SetRentalTime(ushort seconds)
        {
            byte[] mes = new byte[1] { (byte)Enums.SendCommand.SetRentTime };
            var send = mes.Concat(BitConverter.GetBytes(seconds)).ToArray();
            Client.Client.Send(send);
        }
    }
}
