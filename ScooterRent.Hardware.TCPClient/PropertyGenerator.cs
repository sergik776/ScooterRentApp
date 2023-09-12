using ScooterRent.Hardware.HAL;
using System.Net.NetworkInformation;
using static ScooterRent.Hardware.HAL.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScooterRent.Hardware.TCPClient
{
    /// <summary>
    /// Генератор пакета байт из обюектов, для отправки на сервер
    /// </summary>
    internal static class PropertyGenerator
    {
        /// <summary>
        /// Метод преобразования байт в строку для красивого отображения
        /// </summary>
        /// <param name="array">массив байт</param>
        /// <returns></returns>
        public static string ByteArrayToString(this byte[] array)
        {
            return BitConverter.ToString(array);
        }

        /// <summary>
        /// Генерация пакета скорости самоката
        /// </summary>
        /// <param name="speed">Скорость [км/ч]</param>
        /// <returns></returns>
        public static byte[] NewSpeedPacket(int speed)
        {
            RecieveProperty R = RecieveProperty.Speed;
            byte[] bytes = BitConverter.GetBytes(speed);
            //if (BitConverter.IsLittleEndian)
            //    Array.Reverse(bytes);
            return new byte[] { (byte)R }.Concat(bytes).ToArray();
        }

        /// <summary>
        /// Генерация пакета МАК адреса самоката
        /// </summary>
        /// <param name="mac">МАК адрес</param>
        /// <returns></returns>
        public static byte[] NewMACPacket(PhysicalAddress mac)
        {
            RecieveProperty R = RecieveProperty.MAC;
            return new byte[] { (byte)R }.Concat(mac.GetAddressBytes()).ToArray();
        }

        /// <summary>
        /// Генерация пакета состояния самоката
        /// </summary>
        /// <param name="state">Состояние</param>
        /// <returns></returns>
        public static byte[] NewStatePacket(ScooterState state)
        {
            return new byte[] { (byte)RecieveProperty.State, (byte)state };
        }

        /// <summary>
        /// Генерация пакета позиции самоката
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <returns></returns>
        public static byte[] NewPositionPacket(Position position)
        {
            RecieveProperty R = RecieveProperty.Position;
            var N = BitConverter.GetBytes(position.Latitude);
            var E = BitConverter.GetBytes(position.Longitude);
            var res = new byte[] { (byte)R }.Concat(N.Concat(E).ToArray()).ToArray();
            return res;
        }

        /// <summary>
        /// Генерация пакета уровня заряда батареи
        /// </summary>
        /// <param name="bl">Уровень заряда [0-100]</param>
        /// <returns></returns>
        public static byte[] NewBateryLevelPacket(int bl)
        {
            RecieveProperty R = RecieveProperty.BateryLevel;
            byte[] bytes = BitConverter.GetBytes(bl);
            return new byte[] { (byte)R }.Concat(bytes).ToArray();
        }
    }
}
