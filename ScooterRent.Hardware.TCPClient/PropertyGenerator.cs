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
        public static byte[] NewSpeedPacket(sbyte speed)
        {
            RecieveProperty R = RecieveProperty.Speed;
            byte[] bytes = new byte[] { (byte)speed };
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
        public static byte[] NewBateryLevelPacket(byte bl)
        {
            RecieveProperty R = RecieveProperty.BateryLevel;
            byte[] bytes = new byte[] { bl };
            return new byte[] { (byte)R }.Concat(bytes).ToArray();
        }

        /// <summary>
        /// Генерация пакета уровня заряда батареи
        /// </summary>
        /// <param name="bl">Уровень заряда [0-100]</param>
        /// <returns></returns>
        public static byte[] NewRentaltimePacket(ushort bl)
        {
            RecieveProperty R = RecieveProperty.RentalTime;
            byte[] bytes = BitConverter.GetBytes(bl);
            return new byte[] { (byte)R }.Concat(bytes).ToArray();
        }
    }
}
