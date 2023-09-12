using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRent.Hardware.TCPClient
{
    /// <summary>
    /// Генератор полей скутера
    /// </summary>
    internal static class MyStaticGenerators
    {
        public static Random R = new Random();//Рандомайзер

        /// <summary>
        /// Метод генерации случайного enum поля
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomEnumValue<T>() where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            int randomIndex = R.Next(0, values.Length);
            return (T)values.GetValue(randomIndex);
        }

        /// <summary>
        /// Метод генерации позиции
        /// </summary>
        /// <param name="minLatitude">Минимальная широта</param>
        /// <param name="maxLatitude">Максимальная широта</param>
        /// <param name="minLongitude">Минимальная долгота</param>
        /// <param name="maxLongitude">Максимальная долгота</param>
        /// <returns></returns>
        public static Position GenerateRandomPosition(double minLatitude = 0, double maxLatitude = 400, double minLongitude = 0, double maxLongitude = 400)
        {
            var newLatitude = R.Next((int)minLatitude, (int)maxLatitude);
            var newLongitude = R.Next((int)minLongitude, (int)maxLongitude);

            return new Position(newLatitude, newLongitude);
        }

        /// <summary>
        /// Генерирует случайный МАК адрес
        /// </summary>
        /// <returns></returns>
        public static PhysicalAddress GenerateRandomMacAddress()
        {
            Random random = new Random();
            byte[] macAddressBytes = new byte[6];

            // Генерируем случайные байты для MAC-адреса
            R.NextBytes(macAddressBytes);

            // Устанавливаем старший бит первого байта в 0 (указывает на локально управляемый адрес)
            macAddressBytes[0] = (byte)(macAddressBytes[0] & (byte)254);

            return new PhysicalAddress(macAddressBytes);
        }
    }
}
