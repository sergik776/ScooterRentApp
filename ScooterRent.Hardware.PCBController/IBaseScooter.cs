using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    internal interface IBaseScooter
    {
        /// <summary>
        /// МАС адрес самоката
        /// </summary>
        PhysicalAddress MAC { get; }
        /// <summary>
        /// Состояние скутера
        /// </summary>
        public ScooterState State { get; }
        /// <summary>
        /// Событие изменения свойств
        /// </summary>
        public event PropertyHandler? PropertyChanged;
        /// <summary>
        /// Позиция скутера
        /// </summary>
        public Position Position { get; }
        /// <summary>
        /// Уровень заряда батареи
        /// </summary>
        public int BatteryLevel { get; }
        /// <summary>
        /// Текущая скорость
        /// </summary>
        public int Speed { get; }
    }
}
