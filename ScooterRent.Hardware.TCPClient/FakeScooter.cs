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
        public Position Position { get { return _Position; } }
        public PositionGenerate _Position { get; private set; }
        public byte BatteryLevel { get; private set; }
        public sbyte Speed { get; private set; }
        public ushort RentalTime { get; private set; }

        public EndPoint? IP { get { return Tcp.Client.LocalEndPoint; } private set { } }
        public PhysicalAddress MAC { get; private set; }

        public event PropertyHandler? PropertyChanged;

        TcpClient Tcp;
        System.Timers.Timer timer;

        System.Timers.Timer RentalTimer;

        public FakeScooter() 
        {
            Tcp = new TcpClient();
            Tcp.ConnectAsync("127.0.0.1", 8888).Wait();
            MAC = MyStaticGenerators.GenerateRandomMacAddress();
            Tcp.Client.Send(PropertyGenerator.NewMACPacket(MAC));
            

            var p = MyStaticGenerators.GenerateRandomPosition();
            _Position = new PositionGenerate(p.Latitude, p.Longitude);
            BatteryLevel = 100;
            Speed = 0;
            RentalTime = 0;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            Init();
            timer.Start();

            Task.Factory.StartNew(() => { Read(); });
        }

        private void Read()
        {
            while(true)
            {
                byte[] buf = new byte[3];
                Tcp.Client.Receive(buf);
                if (buf[0] == 36)
                {
                    RentalTime = BitConverter.ToUInt16(buf, 1);
                    RentalTimer = new System.Timers.Timer(1000);
                    RentalTimer.Elapsed += RentalTimer_Elapsed;
                    Console.WriteLine($"{MAC} Set Rental Time {RentalTime}");
                    Tcp.Client.Send(PropertyGenerator.NewRentaltimePacket(RentalTime));
                    RentalTimer.Start();
                    _Position.MoveToPoint();
                }
            }
        }

        private void RentalTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if(RentalTime == 0)
            {
                RentalTime = 0;
                Speed = 0;
                Tcp.Client.Send(PropertyGenerator.NewRentaltimePacket(RentalTime));
                RentalTimer.Stop();
                RentalTimer.Dispose();
                Console.WriteLine($"Rental time is End");
            }
            else
            {
                RentalTime--;
                Speed = (sbyte)MyStaticGenerators.R.Next(0, 30);
                BatteryLevel--;
            }
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
            switch (prop)
            {
                case RecieveProperty.BateryLevel:
                    Tcp.Client.Send(PropertyGenerator.NewBateryLevelPacket(BatteryLevel));
                    break;
                case RecieveProperty.RentalTime:
                    Tcp.Client.Send(PropertyGenerator.NewRentaltimePacket(RentalTime));
                break;
                case RecieveProperty.Speed:
                    Tcp.Client.Send(PropertyGenerator.NewSpeedPacket(Speed));
                break;
                case RecieveProperty.Position:
                    Tcp.Client.Send(PropertyGenerator.NewPositionPacket(Position));
                break;
            }
            prop++;
            if((byte)prop >= 4)
            {
                prop = 0;
            }
            //Console.WriteLine(this);
        }

        public override string ToString()
        {
            var q = TimeSpan.FromSeconds(RentalTime);
            return $"Scooter: [MAC: {MAC}] [IP: {IP}] [RentalTime: {q.Hours}:{q.Minutes}:{q.Seconds}] [Position: {Position}] [BateryLevel: {BatteryLevel}%] [Speed: {Speed}km/h]";
        }
    }
}
