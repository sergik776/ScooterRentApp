using ScooterRent.Hardware.HAL.HardwareProtocol;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Делегат изменения свойства
    /// </summary>
    /// <param name="position">Позиция</param>
    public delegate void PropertyHandler(PhysicalAddress mac, RecieveProperty p);

    /// <summary>
    /// Абстрактный класс самоката
    /// </summary>
    public abstract class AScooter : IScooterManager, IScooterClient
    {
        protected TcpClient Client;

        public ScooterState State { get; protected set; }

        public Position Position { get; protected set; }

        public int BatteryLevel { get; protected set; }

        public int Speed { get; protected set; }

        public EndPoint? IP {get { return Client.Client.RemoteEndPoint; } }

        public PhysicalAddress MAC { get; protected set; }

        public event PropertyHandler? PropertyChanged;

        public AScooter(TcpClient c) 
        {
            Client = c;
            Position = new Position(0, 0);
            MAC = new PhysicalAddress(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
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
                    MAC = pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.MAC);
                    break;

                case RecieveProperty.Position:
                    Position = pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Position);
                    break;

                case RecieveProperty.BateryLevel:
                    BatteryLevel = pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.BateryLevel);
                    break;

                case RecieveProperty.Speed:
                    Speed = (int)pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.Speed);
                    break;

                case RecieveProperty.State:
                    State = pack.Value;
                    PropertyChanged?.Invoke(MAC, RecieveProperty.State);
                    break;

                default:
                    
                    break;
            }
            //System.Console.Clear();
            //System.Console.WriteLine(this);
        }

        public abstract bool Lock();
        public abstract bool Unlock();

        public override string ToString()
        {
            return $"Scooter: [MAC: {MAC}] [IP: {IP}] [State: {State}] [Position: {Position}] [BateryLevel: {BatteryLevel}%] [Speed: {Speed}km/h]";
        }
    }
}
