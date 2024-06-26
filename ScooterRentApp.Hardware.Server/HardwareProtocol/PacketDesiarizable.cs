﻿using ScooterRent.Hardware.HAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static ScooterRent.Hardware.HAL.Enums;

namespace ScooterRentApp.Hardware.Server.HardwareProtocol
{
    /// <summary>
    /// Класс рассшифровки пакета от скутера
    /// </summary>
    internal static class PacketDesiarizable
    {
        /// <summary>
        /// Метод расшифровки
        /// </summary>
        /// <param name="data">Массив байт из ТСП потока скутера</param>
        /// <returns>Расшифрованый пакет</returns>
        /// <exception cref="Exception"></exception>
        public static ScooterDataPacket Desiarizable(byte[] data)
        {
            switch((RecieveProperty)data[0])
            {
                case RecieveProperty.MAC:
                    return new ScooterDataPacket(RecieveProperty.MAC, new PhysicalAddress(data.Skip(1).ToArray().SkipLast(10).ToArray()));

                case RecieveProperty.Position:
                    var res = new ScooterDataPacket(RecieveProperty.Position, new Position(
                        BitConverter.ToDouble(data, 1), BitConverter.ToDouble(data, 9)
                        )); return res;

                case RecieveProperty.BateryLevel:
                    return new ScooterDataPacket(RecieveProperty.BateryLevel, (byte)data[1]);

                case RecieveProperty.Speed:
                    return new ScooterDataPacket(RecieveProperty.Speed, (sbyte)data[1]);

                case RecieveProperty.RentalTime:
                    return new ScooterDataPacket(RecieveProperty.RentalTime, BitConverter.ToUInt16(data, 1));

                default: throw new Exception("Битый пакет");
            }
        }
    }
}
