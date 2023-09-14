using ScooterRent.Hardware.HAL;
using ScooterRentApp.Hardware.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.Server.WPF
{
    /// <summary>
    /// Класс вьюхи скутера, унаследован от базового скутера
    /// </summary>
    class ScooterMVVM : INotifyPropertyChanged
    {
        private string _RentalTime;
        public string RentalTime { get { return _RentalTime; } set { _RentalTime = value; OnPropertyChanged(nameof(RentalTime)); } }
        private string _BatteryLevel;
        public string BatteryLevel { get { return _BatteryLevel; } set { _BatteryLevel = value; OnPropertyChanged(nameof(BatteryLevel)); } }
        private string _Speed;
        public string Speed { get { return _Speed; } set { _Speed = value; OnPropertyChanged(nameof(Speed)); } }
        private string _MAC;
        public string MAC { get { return _MAC; } set { _MAC = value; OnPropertyChanged(nameof(MAC)); } }
        private EndPoint? _IP;
        public EndPoint? IP { get { return _IP; } set { _IP = value; OnPropertyChanged(nameof(IP)); } }
        private Position _Position;
        public Position Position { get { return _Position; } set { _Position = value; OnPropertyChanged(nameof(Position)); } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static implicit operator ScooterMVVM(Scooter scooter)
        {
            return new ScooterMVVM()
            {
                IP = scooter.IP,
                BatteryLevel = scooter.BatteryLevel + "%",
                MAC = BitConverter.ToString(scooter.MAC.GetAddressBytes()),
                Position = scooter.Position,
                Speed = scooter.Speed.ToString(),
                RentalTime = scooter.RentalTime.ToString()
            };
        }
    }
}
