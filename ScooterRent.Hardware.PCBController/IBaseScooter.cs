using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL
{
    /// <summary>
    /// Делегат изменения свойства
    /// </summary>
    /// <param name="position">Позиция</param>
    public delegate void PropertyHandler(PhysicalAddress mac, RecieveProperty p);

    public interface IBaseScooter : IScooterClient
    {
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
        public byte BatteryLevel { get; }
        /// <summary>
        /// Текущая скорость
        /// </summary>
        public sbyte Speed { get; }
        /// <summary>
        /// Состояние скутера
        /// </summary>
        public ushort RentalTime { get; }
    }
}
