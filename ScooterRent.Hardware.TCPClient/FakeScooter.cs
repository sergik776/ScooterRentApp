using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.TCPClient
{
    internal class FakeScooter : IBaseScooter
    {
        public Position Position { get; private set; }
        public byte BatteryLevel { get; private set; }
        public sbyte Speed { get; private set; }
        public ushort RentalTime { get; private set; }

        public EndPoint? IP { get; private set; }
        public PhysicalAddress MAC { get; private set; }

        public event PropertyHandler? PropertyChanged;

        TcpClient Tcp;
        System.Timers.Timer timer;

        public FakeScooter() 
        {
            Tcp = new TcpClient();
            Tcp.ConnectAsync("127.0.0.1", 8888).Wait();
            MAC = MyStaticGenerators.GenerateRandomMacAddress();
            Tcp.Client.Send(PropertyGenerator.NewMACPacket(MAC));

            Position = MyStaticGenerators.GenerateRandomPosition();
            BatteryLevel = 100;
            Speed = 0;
            RentalTime = 0;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            Init();
            timer.Start();
        }

        private void Init()
        {
            Tcp.Client.Send(PropertyGenerator.NewBateryLevelPacket(BatteryLevel));
            Thread.Sleep(200);
            Tcp.Client.Send(PropertyGenerator.NewRentaltimePacket(RentalTime));
            Thread.Sleep(200);
            Tcp.Client.Send(PropertyGenerator.NewSpeedPacket(Speed));
            Thread.Sleep(200);
            Tcp.Client.Send(PropertyGenerator.NewPositionPacket(Position));
        }

        RecieveProperty prop;
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Speed++;
            Console.WriteLine(prop);
            switch (prop)
            {
                case RecieveProperty.BateryLevel:
                    Tcp.Client.Send(PropertyGenerator.NewBateryLevelPacket(BatteryLevel));
                Console.WriteLine("BL");
                    break;
                case RecieveProperty.RentalTime:
                    Tcp.Client.Send(PropertyGenerator.NewRentaltimePacket(RentalTime));
                Console.WriteLine("RT");
                break;
                case RecieveProperty.Speed:
                    Tcp.Client.Send(PropertyGenerator.NewSpeedPacket(Speed));
                Console.WriteLine("SP");
                break;
                case RecieveProperty.Position:
                    Tcp.Client.Send(PropertyGenerator.NewPositionPacket(Position));
                Console.WriteLine("PS");
                break;
            }
            prop++;
            if((byte)prop >= 3)
            {
                prop = 0;
            }
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"Scooter: [MAC: {MAC}] [IP: {IP}] [RentalTime: {RentalTime}] [Position: {Position}] [BateryLevel: {BatteryLevel}%] [Speed: {Speed}km/h]";
        }
    }
}
