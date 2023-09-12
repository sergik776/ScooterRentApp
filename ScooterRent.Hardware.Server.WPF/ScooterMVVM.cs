using ScooterRent.Hardware.HAL;
using ScooterRent.Hardware.HAL.HardwareProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    class ScooterMVVM : Scooter, INotifyPropertyChanged
    {
        /// <summary>
        /// Констурктор
        /// </summary>
        /// <param name="c">ТСП поток стутера</param>
        public ScooterMVVM(TcpClient c) : base(c)
        {
            base.PropertyChanged += ScooterMVVM_PropertyChanged;//при изменении состояния скутера вызываем обновление вьюхи
        }

        private void ScooterMVVM_PropertyChanged(RecieveProperty p)
        {
            switch (p)//Определяем тип изменения
            {
                case RecieveProperty.MAC: //вызываем изменения соответствующего свойства в вьюхе
                    OnPropertyChanged(nameof(MAC));
                    break;

                case RecieveProperty.Position:
                    OnPropertyChanged(nameof(Position));
                    break;

                case RecieveProperty.BateryLevel:
                    OnPropertyChanged(nameof(BatteryLevel));
                    break;

                case RecieveProperty.Speed:
                    OnPropertyChanged(nameof(Speed));
                    break;

                case RecieveProperty.State:
                    OnPropertyChanged(nameof(State));
                    break;
            }
        }

        //Заменяем старые свойтва модели на новые свойства с реализацией INotifyPropertyChanged (ключевое слово new)
        public new string State { get { return base.State.ToString(); } set { OnPropertyChanged(nameof(State)); } }
        //Тут позиция не заменяется а добавляется новая, так как в вью нужно получить координаты для точки,
        //а этот стринг для таблицы
        public string PositionS { get { return base.Position.ToString(); } set { OnPropertyChanged(nameof(PositionS)); } }
        public new string BatteryLevel { get { return base.BatteryLevel.ToString() + "%"; } set { OnPropertyChanged(nameof(BatteryLevel)); } }
        public new string Speed { get { return base.Speed.ToString() + " km/h"; } set { OnPropertyChanged(nameof(Speed)); } }
        public new string MAC { get { return base.MAC.ToString(); } set { OnPropertyChanged(nameof(MAC)); } }






        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
