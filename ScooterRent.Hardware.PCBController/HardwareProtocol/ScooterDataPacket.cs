using System.Net.NetworkInformation;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRent.Hardware.HAL.HardwareProtocol
{
    /// <summary>
    /// Пакет полученных данных
    /// </summary>
    public class ScooterDataPacket
    {
        /// <summary>
        /// Тип свойства
        /// </summary>
        public RecieveProperty Property { get; }
        /// <summary>
        /// Значение свойства
        /// </summary>
        public dynamic Value { get; }
        /// <summary>
        /// Конструктор класса пакета получения данных
        /// </summary>
        /// <param name="data">Пакет байт</param>
        public ScooterDataPacket(RecieveProperty property, dynamic value)
        {
            this.Value = value;
            this.Property = property;
        }

        public override string ToString()
        {
            if(Property == RecieveProperty.MAC)
            {
                return $"{Property}: {BitConverter.ToString(((PhysicalAddress)Value).GetAddressBytes())}";
            }
            return $"{Property}: {Value}";
        }
    }
}
